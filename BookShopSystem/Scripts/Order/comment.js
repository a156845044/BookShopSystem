var id = $.getQueryString("orderId");//获取订单编号

var AppComment = new Vue({
    el: '#app-comment',
    data: {
        islogin: islogin,
        commentList: [],//待评价的商品列表
    },
    methods: {
        register: function () {
            $.login(true);
        },
        login: function () {
            $.register(true);
        },
        save: function () {
            $.httpPost("/Order/AddComment", { orderId: id, list: this.commentList }).then(function (req) {
                if (req == true) {
                    alert("评论成功！");
                    setTimeout(e => {
                        window.location.href = "/Order/Index";
                    }, 500);
                } else {
                    alert("操作失败！");
                }
            });

        }

    }
});

//初始化
$(function () {
    initData();
    initCommentHover();//初始化效果
});

/**
 *初始化效果
 */
function initCommentHover() {
    $(document).ready(function () {
        $(".comment-list .item-opinion li").click(function () {
            $(this).prevAll().children('i').removeClass("active");
            $(this).nextAll().children('i').removeClass("active");
            $(this).children('i').addClass("active");

        });
    });
}

//初始化数据
function initData() {
    initBookComment();//初始化图书评论
}

//初始化图书评论
function initBookComment() {
    $.httpPost("/Order/GetInCommendList", { orderId: id }).then(function (req) {
        if (req) {
            AppComment.commentList = req;
            Vue.nextTick(function () {
                initCommentHover();
            });
        }
    });
}