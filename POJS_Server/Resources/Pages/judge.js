function judge() {
    var sorcecode = document.getElementById("sorcecode").value;
    var input = document.getElementById("input").value;
    var output = document.getElementById("output").value;

    if (sorcecode === "" || input === "" || output === "") {
        alert("源代码、输入、输出都不能为空！！！");
        return;
    }

    document.getElementById("result").value = "";

    var request;
    if (window.XMLHttpRequest) {
        request = new XMLHttpRequest();
    } else {
        request = new ActiveXObject('Microsoft.XMLHTTP');
    }

    if (request == null || request === undefined) {
        alert("浏览器版本过低");
        return;
    }
    request.open('POST', '/submit', true);
    request.timeout = 10000;
    request.onreadystatechange = function () {
        if (request.readyState === 4) {
            if (request.status === 200) {
                handleReceiveData(request.responseText);
                request.abort();
            }
            //else {
            //    alert("连接失败");
            //}
        }
    }
    request.onerror = function () {
        alert("连接失败");
    }

    request.send(sorcecode + "#$" + input + "#$" + output);

}

function handleReceiveData(response) {
    var result = document.getElementById("result");
    result.innerText = response;
    alert(response);
}