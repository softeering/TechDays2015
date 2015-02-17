/// <reference path="typings/knockout/knockout.d.ts" />
export declare class viewModelBase {
    private _clearErrorMessageTimeOut;
    errorMessage: KnockoutObservable<string>;
    loading: KnockoutObservable<boolean>;
    constructor();
    protected logError: (message: string) => void;
    protected baseInit(): void;
}
