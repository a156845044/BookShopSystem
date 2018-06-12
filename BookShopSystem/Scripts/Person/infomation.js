var AppPerson = new Vue({
    el: '#app-person',
    data: {
        islogin: islogin,
        professionList: [],
        loginInfo: {}
    },
    methods: {
        register: function () {
            $.login(true);
        },
        login: function () {
            $.register(true);
        },
        save: function () {
            update();
        }
    },
    computed: {

    }
});

$(function () {
    initLoginInfo();//初始化登录信息
    initProfessionList();//初始化职业

    $("#profession-select").on('change', function () {
        AppPerson.loginInfo.ProfessionId = $(this).val();
    });
});

/**
 * 初始化职业
 */
function initProfessionList() {
    $.httpPost("/Admin/Book/GetProfessionList").then(function (req) {
        if (req) {
            AppPerson.professionList = req;
        }
    });
}


//初始化登录信息
function initLoginInfo() {
    $.httpPost("/Home/GetLoginInfo").then(function (req) {
        if (req) {
            AppPerson.loginInfo = req;
        }
    });
}


//更新
function update() {
    $.httpPost("/Person/UpdatePersonInfo", { professionId: AppPerson.loginInfo.ProfessionId }).then(function (req) {
        if (req == true) {
            alert("保存成功！");
        }
        else {
            alert("保存失败！");
        }
    });
}





