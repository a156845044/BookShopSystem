/**
 * 订单
 */
var AppOrder = new Vue({
    el: '#orderlist-main',
    data: {
        islogin: islogin,
        list: [],
    },
    filters: {
        formatDate: function (val) {
            return moment(val).format("lll");
        }
    },
    methods: {
        register: function () {
            $.login(true);
        },
        login: function () {
            $.register(true);
        },
        comment: function (id) {
            window.location.href = "/Order/Comment?orderId=" + id;
        }
    }
});

$(function () {
    initOrderList();//初始化我的订单列表
});


//初始化我的订单列表
function initOrderList() {
    $.httpPost("/Order/GetMyOrderList").then(function (req) {
        if (req) {
            AppOrder.list = req;
        }
    });
}

