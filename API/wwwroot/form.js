

let onSend = async () => {

    let language = document.querySelector("#languageInput").value;
    let text = document.querySelector("#codeInput").value;

   
    document.querySelector("#resultJson").innerText = JSON.stringify(Api.fetchRequestForSumTask(language, text), null, 2);

};
