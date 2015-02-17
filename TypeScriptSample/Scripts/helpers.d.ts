/// <reference path="configuration.d.ts" />
export declare module helpers {
    class stringHelper {
        static endsWith: (str: string, suffix: string) => boolean;
    }
    class DynamoClient {
        private _parameters;
        private _config;
        constructor();
        private createUrl;
        loadDataSource: (code: string) => JQueryPromise<any>;
        addParameter: (key: string, value: string) => void;
    }
}
