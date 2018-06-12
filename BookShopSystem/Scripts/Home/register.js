
var regex = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/

var app = new Vue({
    el: '#app-register',
    data: {
        checked: false,
        account: "",
        pwd: "",
        pwdRepeat:""
    },
    //过滤器
    filters: {
        formatClass: function (val) {
            var h = "";
            if (val == 1) {
                h = "am-form-success am-form-icon am-form-feedback";
            } else if (val == 2) {
                h = "am-form-error am-form-icon am-form-feedback";
            }
            return h;
        }
    },
    //计算属性
    computed: {
        legalSate: function () {
            var val = 0;
            if (this.account != "") {
                if (regex.test(this.account)) {
                    val = 1;
                }
                else {
                    val = 2;
                }
            }
            return val;
        }
    },
    //方法
    methods: {
        save: function () {
            var $this = this;
            $this.$validator.validateAll().then((result) => {
                if (!result) {
                    alert($this.errors.items[0].msg);
                }
                else {
                    if ($.trim($this.pwd) != $.trim($this.pwdRepeat)) {
                        alert("两次密码输入不一致！");
                    }
                    else {
                        var param = { Account: $this.account, Pwd: $this.pwd };
                        $.httpPost("/Home/RegisterMember", { model: param }).then(function (req) {
                            if (req.Status) {
                                layer.msg('恭喜您，注册成功！',
                                    { icon: 4 });
                                setTimeout(e => {
                                    $.goIndexPage();
                                }, 500);
                            }
                            else {
                                alert(req.Msg);
                            }

                        });
                    }
                }
            });
        }
    }
});
