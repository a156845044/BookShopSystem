/*
* 搜索列表
*/
var categoryId = $.getQueryString("categoryId");//获取分类编号
var key = $.getQueryString("key");

var count = 0;


if (categoryId == undefined) {
    categoryId = 0;
}

if (key == undefined) {
    key = "";
}
else {
    key = decodeURI(key);
}

console.log(key);

var AppSearch = new Vue({
    el: '#app-search',
    data: {
        list: [],//分类
        pager: { PageCount: 0, PageIndex: 1, PageSize: 10, CategoryId: categoryId, Key: key }
    }

});

//初始化
$(function () {
    initData();
   
});

//初始化数据
function initData() {
    initSearchList();//初始化搜索列表
}

//初始化搜索列表
function initSearchList() {
    $.httpPost("/Home/GetSearchList", AppSearch.pager).then(function (req) {
        if (req) {
            AppSearch.list = req.Data;
            AppSearch.pager.PageCount = req.PageCount;
            count++;
            if (count <= 1) {
                $("#pager").page({
                    pages: AppSearch.pager.PageCount,
                    jump: function (context, first) {
                        AppSearch.pager.PageIndex = context.option.curr;
                        initSearchList();//初始化图书评论
                    }
                });
            }
        }
    });
}

