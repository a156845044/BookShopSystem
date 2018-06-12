var AppPay = new Vue({
    el: '#app-pay',
    data: {
        islogin: islogin,
        address: "南京市雨花台区铁心桥龙西路310号",
        tel: "13811111111",
        contacts:"小明",
        productList: [],//图片列表
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
        pay: function () {
            if (this.islogin) {
                buy();//下单
            }
            else {
                $.login(true);
            }
        }
    },
    computed: {
        totalPrice: function () {
            var total = 0;
            $.each(this.productList, function (i, item) {
                total += item.UnitPrice * item.BuyNum;
            });
            return total.toFixed(2);
        }
    }
});

$(function () {
    initProductList();//初始化购买商品列表
});


//初始化购买商品列表
function initProductList() {
    var payInfoList = $.getbuyInfo();
    if (payInfoList.length > 0) {
        $.httpPost("/Order/GetPayProductList", { payInfoList: payInfoList }).then(function (req) {
            if (req) {
                AppPay.productList = req;
            }
        });
    }
}

//下单
function buy() {
    var list = [];

    $.httpPost("/Order/AddOrder", { list: AppPay.productList, address: AppPay.address, tel: AppPay.tel, contacts: AppPay.contacts  }).then(function (req) {
        if (req == true) {
            window.location.href = "/Order/PaySuccess";
        }
        else {
            alert("下单失败！");
        }
    });
}
