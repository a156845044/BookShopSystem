window.alert = function (e) {
    layer.msg(e);
};

/**
 * 全局对象
 * 通用业务逻辑放在该全局对象中
 * @namespace 通用业务方法库
 */
var globalMethod = (function (window) {

    //关闭一个iframe层
    var close = function (opts) {
        var defopts = {
            reload: false
        };
        var options = $.extend({}, defopts, opts);
        try {
            var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
            parent.layer.close(index);
        } catch (e) {
            if (options.reload && window.opener && !window.opener.closed) {
                if (window.opener.pageInit && Object.prototype.toString.call(window.opener.pageInit.initDataTable) === '[object Function]') {
                    window.opener.pageInit.initDataTable({});
                }
            }
            window.opener = null;
            window.close();
        }
    };

    //弹出提示框
    var alert = function (content, opts) {
        var defopt = {
            icon: 1,
            close: false,
            reload: false,
            reLogin: false
        };
        var options = $.extend(defopt, opts);
        layer.alert(content, {
            icon: options.icon,
            title: "提示",
            shade: [0.3, "#FFF"],
            closeBtn: 0,
            yes: function (index) {
                layer.close(index);
                if (options.close) {
                    close(options);
                }
                if (options.reLogin) {
                    window.location.reload();
                }
            }
        });
    };

    /**
     * 请求地址
     * @param {any} url 地址
     * @param {any} data 传输数据
     * @param {any} method 方法 post  get
     * @param {any} dataType 数据类型
     * @param {any} async 是否异步
     */
    var ajaxRequest = function (url, data, method, dataType, async) {
        if (method == undefined) {
            method = "post";
        } else {
            method = method.toUpperCase();
        }
        if (typeof async == 'undefined') {
            async = true;
        }
        var requestVerificationToken = $(":hidden[name='__RequestVerificationToken']").val();
        var options = {
            url: url,
            dataType: dataType || "json",
            cache: false,
            type: method,
            data: data,
            async: async
        };

        $.extend(options, {
            beforeSend: function () {
                layer.load();
            }, complete: function () {
                layer.closeAll('loading');
            }
        });

        $.extend(options.data, {
            "__RequestVerificationToken": requestVerificationToken,
            "requestType": "AjaxRequest"
        });
        return $.ajax(options);
    };

    //弹出confirm确认框
    var confirm = function (msg, yes, no) {
        layer.open({
            closeBtn: 0,
            content: msg,
            btn: ['确认', '取消'],
            shade: [0.3, "#FFF"],
            yes: function () {
                if (!!yes) {
                    yes();
                    //$.close();
                }
            },
            no: function () {
                if (!!no) {
                    no();
                    //$.close();
                }
            }
        });
    }

    //打开一个iframe层
    var open = function (opts) {
        //默认配置
        var defopts = {
            title: "信息",
            width: "750px",
            height: "530px",
            params: {},
            url: "",
            shadeClose: false,
            callback: null
        };

        var option = $.extend({}, defopts, opts);
        var params = $.param(option.params);
        if ($.trim(params) !== "") {
            option.url += "?" + params;
        }
        var urlReg = /Admin|home/i;
        var isWindowLayer = urlReg.test(opts.url);
        if (isWindowLayer) {
            //iframe层-父子操作
            layer.open({
                type: 2,
                title: option.title,
                skin: "layui-layer-lan",
                offset: "18px",
                area: [option.width, option.height],
                fix: false, //不固定
                maxmin: true,
                shade: [0.2, "#FFF"],
                shadeClose: option.shadeClose, //控制是否点击遮罩区域关闭dialog窗口
                content: option.url,
                end: option.callback,
                moveOut: true                  //是否允许拖拽到窗口外
            });
        } else {
            var iWidth = parseInt(option.width),
                iHeight = parseInt(option.height);
            var iLeft = (window.screen.width - 10 - iWidth) / 2; //获得窗口的水平位置;  
            var iTop = (window.screen.height - 30 - iHeight) / 2; //获得窗口的垂直位置; 
            window.open(option.url, option.url, 'height=' + iHeight + ',width=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',location=no,menubar=no,resizable=no,scrollbars=yes,status=no,titlebar=no,toolbar=no');
        }
    };

    //获取Url值
    var getQueryString = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return r[2];
        return null;
    };


    //计算两个日期相差的天数
    var dateDiff = function (dtStart, dtEnd, strInterval) {
        var tempDate = new Date();
        if (typeof dtStart === 'string') {
            dtStart = stringToDate(dtStart);
        }
        if (typeof dtEnd === 'string') {
            dtEnd = stringToDate(dtEnd);
        }
        switch (strInterval) {
            case 's':
                tempDate = Math.abs(parseInt((dtEnd - dtStart) / 1000));
                break;
            case 'n':
                tempDate = Math.abs(parseInt((dtEnd - dtStart) / 60000));
                break;
            case 'h':
                tempDate = Math.abs(parseInt((dtEnd - dtStart) / 3600000));
                break;
            case 'd':
                tempDate = Math.abs(parseInt((dtEnd - dtStart) / 86400000));
                break;
            case 'w':
                tempDate = Math.abs(parseInt((dtEnd - dtStart) / (86400000 * 7)));
                break;
            case 'm':
                tempDate = Math.abs((dtEnd.getMonth() + 1) + ((dtEnd.getFullYear() - dtStart.getFullYear()) * 12) - (dtStart.getMonth() + 1));
                break;
            case 'y':
                tempDate = Math.abs(dtEnd.getFullYear() - dtStart.getFullYear());
                break;
        }
        return tempDate;
    };

    //日期计算
    var addDate = function (dtTmp, Number, strInterval) {
        if (typeof dtTmp === 'string') {
            dtTmp = stringToDate(dtTmp);
        }
        var newDate = new Date();
        switch (strInterval) {
            case 's':
                newDate = new Date(Date.parse(dtTmp) + (1000 * Number));
                break;
            case 'n':
                newDate = new Date(Date.parse(dtTmp) + (60000 * Number));
                break;
            case 'h':
                newDate = new Date(Date.parse(dtTmp) + (3600000 * Number));
                break;
            case 'd':
                newDate = new Date(Date.parse(dtTmp) + (86400000 * Number));
                break;
            case 'w':
                newDate = new Date(Date.parse(dtTmp) + ((86400000 * 7) * Number));
                break;
            case 'q':
                newDate = new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number * 3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
                break;
            case 'm':
                newDate = new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
                break;
            case 'y':
                newDate = new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
                break;
        }
        return newDate;
    };

    //小提示
    var tip = function (msg, t) {
        layer.open({
            content: msg,
            time: t || 2
        });
    }

    /**
      * 登录
      * @param {any} isReturn 是否要返回
      * @param {any} returnURL 指定的要返回地址
      */
    var login = function (isReturn, returnURL) {
        var url = "/Home/Login";
        if (isReturn) {
            var temp = window.location.href;
            if (arguments.length > 1) {
                temp = returnURL;
            }
            url = url + "?returnURL=" + encodeURI(temp);
        }
        window.location.href = url;
    };

    /**
     * 注册
     * @param {any} isReturn 是否要返回
     * @param {any} returnURL 指定的要返回地址
     */
    var register = function (isReturn, returnURL) {
        var url = "/Home/Register";
        if (isReturn) {
            var temp = window.location.href;
            if (arguments.length > 1) {
                temp = returnURL;
            }
            url = url + "?returnURL=" + encodeURI(temp);
        }
        window.location.href = url;
    };

    /**
     * 返回主页
     */
    var goIndexPage = function () {
        var url = "/Home/Index";
        var prev = getQueryString("returnURL");
        if (prev != undefined && prev != "") {
            url = decodeURI(prev);
        }
        window.location.href = url;
    };

    //立即购买
    var buyimmediately = function (id, num) {
        var list = [];
        var item = { ProductId: id, BuyNum: num };
        list.push(item);
        sessionStorage.BuyInfo = JSON.stringify(list);
        window.location.href = "/Order/Pay";
    };

    //获取购买信息
    var getbuyInfo = function () {
        var json = [];
        if (sessionStorage.BuyInfo) {
            if (sessionStorage.BuyInfo != "") {
                json = JSON.parse(sessionStorage.BuyInfo);
            }
        }
        return json;
    };

    /**
     * 搜索
     * @param {any} categoryId 分类编号
     * @param {any} key 主键
     */
    var goSearch = function (categoryId, key) {
        if (key == undefined) {
            key = "";
        }
        else {
            key = encodeURI(key);
        }
        var url = "/Home/Search";
        url = url + "?categoryId=" + categoryId;
        url = url + "&key=" + key;
        window.location.href = url;
    }

    return {
        httpPost: ajaxRequest,
        alert: alert,
        confirm: confirm,
        open: open,
        close: close,
        addDate: addDate,
        dateDiff: dateDiff,
        getQueryString: getQueryString,
        tip: tip,
        login: login,
        register: register,
        goIndexPage: goIndexPage,
        buyimmediately: buyimmediately,
        getbuyInfo: getbuyInfo,
        goSearch: goSearch
    };

})(window);

$.extend(globalMethod);

