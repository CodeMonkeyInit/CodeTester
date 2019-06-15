class CodeQuality{
    constructor(){
        this.codeAnalysisResult = {isSuccessful: false};
        this.codeExecutionResult = {wasSuccessful: false};
        this.score = 0;
    }
}


class Api {
    

    /**
     * 
     * @param language
     * @param text
     * @returns {Promise<CodeQuality>}
     */
    static async fetchRequestForSumTask(language, text) {
        let response = await fetch(Api.getCodeQualityPath(), {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify({
                ExecutionData: Api.sumTaskData,
                language,
                text
            })
        });

        return await response.json();
    }

    static getCodeQualityPath() {
        return this.apiPath + this.codeQualityPath;
    }
}

Api.apiPath = "https://localhost:5003/api";
Api.codeQualityPath = "/CodeQuality";



Api.sumTaskData = [
    {InputData: "20 10", OutputData: "30"},
    {InputData: "1 1", OutputData: "2"},
    {InputData: "2 2 2", OutputData: "6"},
    {InputData: "4 4", OutputData: "8"}
];

Api.supportedLanguages = {
    Js: 1,
    Php: 2,
    Pascal: 3,
    CPlusPlus: 4
};