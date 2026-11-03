function callMethod() {
    DotNet.invokeMethodAsync('MSA_Booking', 'GetValueFromMethod').then(data =>
    {
        alert('Message from method --'+data);
    })
}

//callInstanceMethod
function callInstanceMethod(instanceObject) {
    instanceObject.invokeMethodAsync( 'GetValueFromInstance').then(data => {
        alert('Message from method --' + data);
    })
}

window.initChoices = (element) => {
    if (!element) return;

    // nếu đã init thì bỏ qua
    if (element.dataset.choicesInitialized === "true") return;

    new Choices(element, {
        searchEnabled: true,
        itemSelectText: ''
    });

    element.dataset.choicesInitialized = "true";
};