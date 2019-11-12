export class Project {
    id: string;

    name: string;

    description: string;

    isCompleted: boolean;

    asvsLevel: number;

    users: UserRole[];

    workflowElementCategories: WorkflowElementCategory[];

    sslLabsData: any[];

    timeCreated: Date

    timeLastEdit: Date
}


export class WorkflowElementCategory {
    CategoryId: string;

    Name: string;

    Description: string;

    WorflowElements: WorkflowElement[];
}

export class WorkflowElement {
    ElementId: string;

    Name: string;

    Description: string;

    Explanation: string;

    IsDone: boolean;

    IsRelevant: boolean;
}

export class UserRole {
    UserId: string;

    Name: string;

    Role: string;
}
