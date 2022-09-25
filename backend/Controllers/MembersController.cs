using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prid_tuto.Models;

namespace prid_tuto.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly MsnContext _context;
        private readonly IMapper _mapper;

        /*
        Le contrôleur est instancié automatiquement par ASP.NET Core quand une requête HTTP est reçue.
        Le paramètre du constructeur recoit automatiquement, par injection de dépendance, 
        une instance du context EF (MsnContext).
        */
        public MembersController(MsnContext context ,IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetAll() {
            // Récupère une liste de tous les membres
        return _mapper.Map<List<MemberDTO>>(await _context.Members.ToListAsync()); ;
        }

        [HttpGet("{pseudo}")]
        public async Task<ActionResult<MemberDTO>> GetOne(string pseudo) {
            // Récupère en BD le membre dont le pseudo est passé en paramètre dans l'url
            var member = await _context.Members.FindAsync(pseudo);
            // Si aucun membre n'a été trouvé, renvoyer une erreur 404 Not Found
            if (member == null)
                return NotFound();
            // Retourne le MemberDTO
            return _mapper.Map<MemberDTO>(member);
        }

        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member member) {
            // Ajoute le nouveau membre au contexte EF
            _context.Members.Add(member);
            // Sauve les changements
            await _context.SaveChangesAsync();

            // Renvoie une réponse ayant dans son body les données du nouveau membre (3ème paramètre)
            // et ayant dans ses headers une entrée 'Location' qui contient l'url associé à GetOne avec la bonne valeur 
            // pour le paramètre 'pseudo' de cet url.
            return CreatedAtAction(nameof(GetOne), new { pseudo = member.Pseudo }, member);
        }

        [HttpPut]
        public async Task<IActionResult> PutMember(Member member) {
            // Vérifie si le membre à mettre à jour existe bien en BD
            var exists = await _context.Members.CountAsync(m => m.Pseudo == member.Pseudo) > 0;
            // Si aucun membre n'a été trouvé, renvoyer une erreur 404 Not Found
            if (!exists)
                return NotFound();
            // Ajoute le membre reçu en paramètre au contexte et force son état à "Modified" pour qu'EF fasse un update
            _context.Entry(member).State = EntityState.Modified;
            // Sauve les changements
            await _context.SaveChangesAsync();
            // Retourne un statut 204 avec une réponse vide
            return NoContent();
        }
        
        [HttpDelete("{pseudo}")]
        public async Task<IActionResult> DeleteMember(string pseudo) {
            // Récupère en BD le membre à supprimer
            var member = await _context.Members.FindAsync(pseudo);
            // Si aucun membre n'a été trouvé, renvoyer une erreur 404 Not Found
            if (member == null)
                return NotFound();
            // Indique au contexte EF qu'il faut supprimer ce membre
            _context.Members.Remove(member);
            // Sauve les changements
            await _context.SaveChangesAsync();
            // Retourne un statut 204 avec une réponse vide
            return NoContent();
        }

    }
}
