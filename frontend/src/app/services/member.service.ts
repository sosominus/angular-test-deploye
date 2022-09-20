import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Member } from '../models/member';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class MemberService {
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

    getAll(): Observable<Member[]> {
        return this.http.get<Member[]>(`${this.baseUrl}api/users`)
            .pipe(map(res => res.map(m => new Member(m))));
    }
}