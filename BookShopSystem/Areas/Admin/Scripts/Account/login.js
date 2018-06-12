

var app = new Vue({
    el: '#login',
    data: {
        loginModel:
        {
            account: "",
            pwd: ""
        }
    },
    methods: {
        login: function () {
            var $this = this;
            $this.$validator.validateAll().then((result) => {
                if (!result) {
                    alert($this.errors.items[0].msg);
                }
                else {
                    $.httpPost("/Admin/Account/SubmitLogin", { account: $this.loginModel.account, pwd: $this.loginModel.pwd }).then(function (req) {
                        if (req.Status) {
                            location.href = "/Admin/Account/Index";
                        }
                        else {
                            alert(req.Msg);
                        }

                    });
                }
            });
        }
    }
});
