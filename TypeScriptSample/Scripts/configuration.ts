
export module configuration {
	export class configValues {
		private static _dynamoBaseUrl:string = "http://dynamo";

		// static method
		public static getDynamoBaseURL = () => {
			return configValues._dynamoBaseUrl;
		}
	}
}