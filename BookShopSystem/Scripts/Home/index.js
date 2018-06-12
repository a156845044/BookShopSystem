/*
* 商城首页
*/

var App = new Vue({
    el: '#app-container',
    data: {
        islogin: islogin,
        categoryList: [],//分类
        recommendList: [],//推荐
        literatureList: [],//文学
        computerList: [],//码农世界
        economicsList: [],//经管
        gameList: [],//游戏
        shopingcart: Master.shopingcart
    },
    filters: {
        formatStyleOne: function (val) {
            var h = "";
            switch (val) {
                case 0:
                    h = "am-u-sm-7 am-u-md-4 text-two sug";
                    break;
                case 1:
                    h = "am-u-sm-7 am-u-md-4 text-two";
                    break;
                case 2:
                    h = "am-u-sm-3 am-u-md-2 text-three big";
                    break;
                case 3:
                    h = "am-u-sm-3 am-u-md-2 text-three sug";
                    break;
                case 4:
                    h = "am-u-sm-3 am-u-md-2 text-three ";
                    break;
                case 5:
                    h = "am-u-sm-3 am-u-md-2 text-three last big ";
                    break;
                default:
                    h = "am-u-sm-7 am-u-md-4 text-two sug";
                    break;
            }
            return h;
           
        },
        formatStyleTwo: function (val) {
            var h = "";
            switch (val) {
                case 0:
                    h = "am-u-sm-4 text-four";
                    break;
                case 1:
                    h = "am-u-sm-4 text-four sug";
                    break;
                case 2:
                    h = "am-u-sm-6 am-u-md-3 text-five big";
                    break;
                case 3:
                    h = "am-u-sm-6 am-u-md-3 text-five ";
                    break;
                case 4:
                    h = "am-u-sm-6 am-u-md-3 text-five sug";
                    break;
                case 5:
                    h = "am-u-sm-6 am-u-md-3 text-five big";
                    break;
                default:
                    h = "";
                    break;
            }
            return h;

        }
    },
    methods: {
        register: function () {
            $.register(true);
        },
        login: function () {
            $.login(true);
        },
        search: function (id) {
            console.log("ddd");
            $.goSearch(id, Master.key);
        }
    }
});

//初始化
$(function () {
    initData();
});

//初始化数据
function initData() {
    initCategory();//初始化分类
    initBookRecommend();//初始化好书推荐
    initBookByCategoryId(1);//文学
    initBookByCategoryId(5);//码农世界
    initBookByCategoryId(4);//经管
    initBookByCategoryId(7);//游戏
}

//初始化分类
function initCategory() {
    $.httpPost("/Home/GetIndexCategoryList").then(function (req) {
        if (req) {
            App.categoryList = req;
            Vue.nextTick(function () {
                initCategoryHover();
            });
        }
    });
}

//初始化效果
function initCategoryHover() {
    $("li").hover(function () {
        $(".category-content .category-list li.first .menu-in").css("display", "none");
        $(".category-content .category-list li.first").removeClass("hover");
        $(this).addClass("hover");
        $(this).children("div.menu-in").css("display", "block");
    }, function () {
        $(this).removeClass("hover");
        $(this).children("div.menu-in").css("display", "none");
    });
}

//初始化好书推荐
function initBookRecommend() {
    $.httpPost("/Home/GetBookRecommendList").then(function (req) {
        if (req) {
            App.recommendList = req;
        }
    });
}

/**
 * 根据分类初始化图书列表
 * @param {any} categoryId 分类编号
 * @param {any} data 要初始化的对象
 */
function initBookByCategoryId(categoryId) {
    $.httpPost("/Home/GetBookBlockList", { categoryId: categoryId}).then(function (req) {
        if (req) {
            if (categoryId == 1) {
                App.literatureList = req;
            }
            else if (categoryId == 5) {
                App.computerList = req;
            }
            else if (categoryId == 4) {
                App.economicsList = req;
            }
            else if (categoryId == 7) {
                App.gameList = req;
            }
        }
    });
}