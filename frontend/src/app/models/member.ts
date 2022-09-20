export class Member {

    pseudo?: string;
    password?: string;
    fullName?: string;
    birthDate?: string;

    constructor(data: any) {
        if (data) {
            this.pseudo = data.pseudo;
            this.password = data.password;
            this.fullName = data.fullName;
            this.birthDate = data.birthDate &&
                data.birthDate.length > 10 ? data.birthDate.substring(0, 10) : data.birthDate;
        }
    }
}