var id = $.getQueryString("id");//获取主键编号
var Appintroduction = new Vue({
    el: '#app-introduction',
    data: {
        islogin: islogin,
        imgList: [],//图片列表
        cover: "",
        buyNum: 1,
        product: {

        },//商品信息
        statistics: {

        },//统计信息
        recommendList: [],
        pager: { PageCount:0,PageIndex: 1, PageSize: 10, ProductId: id },
        commentList:[]//评论
    },
    watch: {
        buyNum: function (val) {
            if (val > this.product.Quantity) {
                this.buyNum = this.product.Quantity;
            }
            if (val <= 0) {
                this.buyNum = 1;
            }
        }
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
        buy: function () {
            if (this.islogin) {
                $.buyimmediately(id, this.buyNum);
            }
            else {
                $.login(true);
            }
        },
        addshoping: function () {
            if (this.islogin) {
                console.log(this.buyNum);
               $.buyimmediately(id, this.buyNum);
            }
            else {
                $.login(true);
            }
        }
    }
});

//初始化
$(function () {
    initData();
    initJqzoom();

    $("#pager").page({
        pages: Appintroduction.pager.PageCount,
        jump: function (context, first) {
            Appintroduction.pager.PageIndex = context.option.curr;
            initBookComment();//初始化图书评论
        }
    });
});

//初始化数据
function initData() {
    if (id != undefined && id != "") {
        initBookInfo();//初始化商品详细
    }
    initBookRecommend();//初始化好书推荐
    initBookComment();//初始化图书评论
}

//初始化放大器
function initJqzoom() {
    $(".jqzoom").imagezoom();
    $("#thumblist li a").click(function () {
        $(this).parents("li").addClass("tb-selected").siblings().removeClass("tb-selected");
        $(".jqzoom").attr('src', $(this).find("img").attr("mid"));
        $(".jqzoom").attr('rel', $(this).find("img").attr("big"));
    });
}

//初始化商品详细
function initBookInfo() {
    $.httpPost("/Product/GetProductInfo", { productId: id }).then(function (req) {
        if (req.Status) {
            Appintroduction.imgList = req.Data.ImgList;
            Appintroduction.product = req.Data.Product;
            Appintroduction.product.PublishDate = moment(Appintroduction.product.PublishDate).format("YYYY-MM-DD")
            var item = { sales: req.Data.Sales, monthSales: req.Data.MonthSales, commentCount: req.Data.CommentCount };
            Appintroduction.statistics = item;
            if (Appintroduction.imgList.length > 0) {
                Appintroduction.cover = Appintroduction.imgList[0].FilePath;
            }
            Vue.nextTick(function () {
                initJqzoom();
            });
        } else {
            alert(req.Msg);
        }
    });
}


//初始化好书推荐
function initBookRecommend() {
    $.httpPost("/Product/GetBookRecommendList", { productId:id}).then(function (req) {
        if (req) {
            Appintroduction.recommendList = req;
        }
    });
}



//初始化图书评论
function initBookComment() {
    $.httpPost("/Product/GetCommentList", Appintroduction.pager).then(function (req) {
        if (req) {
            Appintroduction.commentList = req.Data;
            Appintroduction.pager.PageCount = req.PageCount;
        }
    });
}