///url,width,height,modalness,args
function popWin(setting) {
    var url = setting.url;
    /*
    do {
        url = url.replace('+', '%2b').replace('/', '%2f');
    }
    while (url.indexOf('+') >= 0 || url.indexOf('/') >= 0);
    */
    var left = 0, top = 0;
    var feature1 = 'scroll=no,toolbar=no,resizable=no,location=no,status=yes,';
    var feature2 = 'scroll=no;toolbar=no;resizable=no;location=no;status=yes;';
    if (typeof setting.width != 'undefined') {
        left = (window.screen.width - setting.width) / 2;
        feature1 += 'left=' + left + ',width=' + setting.width + ',';
        feature2 += 'dialogWidth=' + setting.width + 'px;dialogLeft='+left+"px;";
    }
    if (typeof setting.height != 'undefined') {
        top = (window.screen.height - setting.height) / 4;
        feature1 += 'top=' + top + ',height=' + setting.height;
        feature2 += 'dialogHeight=' + setting.height + 'px;dialogTop=' + top + "px;";
    }
    var popWindow = null;
    if (setting.isModal) {
        popWindow = window.showModalDialog(url, setting.args ? setting.args : 'window', feature2);
    }
    else {
        popWindow = window.open(url, '', feature1);
        popWindow.isPopup = true;
    }
    return popWindow;
};
function showFullScreenDialog(url) {
    if (url == '') { return false; }
    var width = screen.availWidth-12;
    var height = screen.availHeight-25;
    window.open(url, "", "height=" + height + ", width=" + width + ", left=0, top=0, toolbar =no, menubar=no, scrollbars=yes, resizable=yes, location=no, status=no");
    return false;
};
Array.prototype.contains = function (i) {
    return RegExp("\\b" + i + "\\b").test(this);
};
//String Builder
function stringBuilder() {
    this.__S_B = new Array();
};
stringBuilder.prototype.append = function (value) {
    this.__S_B.push(value);
};
stringBuilder.prototype.appendNewLine = function (value) {
    this.__S_B.push('');
    this.__S_B.push(value);
};
stringBuilder.prototype.toString = function (splitExp) {
    return (typeof splitExp != 'undefined' ? this.__S_B.join(splitExp) : this.__S_B.join(''));
};

String.prototype.format = function() {
    var args = arguments;
    return this.replace(/{(\d{1})}/g, function() {
        return args[arguments[1]];
    });
};
String.prototype.ltrim = function (s) {
    if (!s) {
        return this.replace(/(^\s*)/g, '');
    }
    var i, l = this.length;
    for (i = 0; i < l; i++) {
        if (this.charAt(i) != s) break;
    }
    return this.substring(i, l);
};
String.prototype.rtrim = function (s) {
    if (!s) {
        return this.replace(/(\s*$)/g, ''); 
    }
    var i, l = this.length;
    for (i = l - 1; i >= 0; i--) {
        if (this.charAt(i) != s) break;
    }
    return this.substring(0, i + 1);
};
String.prototype.trim = function (s) {
    if (!s) {
        return this.replace(/(^\s*)|(\s*$)/g, ''); 
    }
    return this.ltrim(s).rtrim(s);
};
String.prototype.quoted = function () {
    return this.replace(/\'/g, '\\\'').replace(/\"/g, '\\"').replace(/\n/g, '').replace(/\r/g, '').replace(/\t/g, '');
};
//cut
String.prototype.cut = function (len) {
    var position = 0;
    var result = [];
    var tale = '';
    for (var i = 0; i < this.length; i++) {
        if (position >= len) {
            tale = '...';
            break;
        }
        if (this.charCodeAt(i) > 255) {
            position += 2;
            result.push(this.substr(i, 1));
        }
        else {
            position++;
            result.push(this.substr(i, 1));
        }
    }
    return result.join("") + tale;
};
//Template translator
window.__templateCache = {};
function templateTrans(template, json) {
    var foo = !/\W/.test(template) ?
         (__templateCache[template] = (__templateCache[template] || templateTrans(document.getElementById(template).innerHTML)))
         :
         (new Function("obj",
                            "var p=[];" +
                            "with(obj){p.push('" +
                             template.replace(/[\r\t\n]/g, " ")
                            .replace(/'(?=[^#]*#>)/g, "\t")
                            .split("'").join("\\'")
                            .split("\t").join("'")
                            .replace(/<#=(.+?)#>/g, "',$1,'")
                            .split("<#").join("');")
                            .split("#>").join("p.push('")
                            + "');}return p.join('');"));
    return json ? foo(json) : foo;
};

function resetDialogHeight() {
    var ua = navigator.userAgent.toUpperCase();
    var h = document.body.offsetHeight;
    var w = document.body.offsetWidth;
    if (ua.lastIndexOf("MSIE 7.0") > 0 || ua.lastIndexOf("MSIE 8.0") > 0) {return;}
    if (ua.lastIndexOf("MSIE 6.0") > 0) {
        window.dialogHeight = (h + 70) + "px";
        window.dialogWidth = (w + 20) + "px";
    }
};