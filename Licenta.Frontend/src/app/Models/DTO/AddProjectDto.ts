export class AddProjectDto {
    name: string;
    description: string;
    startDate: Date;
    endDate: Date;
    ownerId: string;

    constructor()
    {
        this.name = '';
        this.description = '';
        this.startDate = new Date();
        this.endDate = new Date();
        this.ownerId = '';
    }
}