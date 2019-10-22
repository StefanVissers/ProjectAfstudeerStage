export class Project {
    Id: string;

    Name: string;

    Description: string;

    IsCompleted: boolean;

    ASVSLevel: number;

    WorkflowElementCategories: WorkflowElementCategory[];
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

export enum ASVSLevel {
    LevelOne = 1,
    LevelTwo = 2,
    LevelThree = 3
}

export class EnumEx {
    static getNamesAndValues<T extends number>(e: any) {
        return EnumEx.getNames(e).map(n => ({ name: n, value: e[n] as T }));
    }

    static getNames(e: any) {
        return Object.keys(e).filter(k => typeof e[k] === "number") as string[];
    }
}
