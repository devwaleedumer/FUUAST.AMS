export class ApplicantInfoRequest {

    userName?: string;   
    email?: string;
    verificationStatusEid:number | undefined


    constructor() {
        this.userName = "";       
        this.email = "";
        this.verificationStatusEid=0;    
}
}
