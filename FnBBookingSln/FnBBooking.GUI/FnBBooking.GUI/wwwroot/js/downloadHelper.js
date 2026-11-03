// Thêm vào wwwroot/js/app.js hoặc trong thẻ <script> ở _Host.cshtml / App.razor

/**
 * Trigger trình duyệt download file từ base64 string.
 * Được gọi từ Blazor: JS.InvokeVoidAsync("downloadFileFromBase64", fileName, contentType, base64)
 */
window.downloadFileFromBase64 = (fileName, contentType, base64) => {
    const byteChars = atob(base64);
    const byteNumbers = new Array(byteChars.length);
    for (let i = 0; i < byteChars.length; i++) {
        byteNumbers[i] = byteChars.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: contentType });

    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url);
};
