var AppPerson = new Vue({
    el: '#app-person',
    data: {
        islogin: islogin,
        day: moment().format('DD'),
        week: moment().format('dddd'),
        month: moment().format('YYYY.MM'),
        loginInfo: {}
    },
    methods: {
        register: function () {
            $.login(true);
        },
        login: function () {
            $.register(true);
        }
    },
    computed: {

    }
});

$(function () {
    initLoginInfo();//初始化登录信息
});


//初始化登录信息
function initLoginInfo() {
    $.httpPost("/Home/GetLoginInfo").then(function (req) {
        if (req) {
            AppPerson.loginInfo = req;
        }
    });
}




