

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
                    $.httpPost("/Home/SubmitLogin", { account: $this.loginModel.account, pwd: $this.loginModel.pwd }).then(function (req) {
                        if (req.Status) {
                            $.goIndexPage();
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
