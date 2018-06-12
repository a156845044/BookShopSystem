
/**
* 人员管理
*/


var User = new Vue({
    el: '#app-user',
    data: {
        key: ""
    },
    methods: {
        search: function () {
            initTable();//初始化表格
        }
    }
});

$(function () {
    initTable();//初始化表格
});

/**
 *初始化表格
 */
function initTable() {
    $('#table-list').dataTable({
        "sort": false,
        "autoWidth": false,
        "destroy": true,
        "processing": false,
        "serverSide": false,
        "searching": false,
        "bLengthChange": false,
        "ajax": {
            "url": "/Admin/User/GetUserList",
            "type": "POST",
            "data": function (obj) {
                obj.__RequestVerificationToken = $("input[name='__RequestVerificationToken']").val();
                obj.key = User.key;
            },
            "sortable": false
        },
        "columns": [
            {
                "data": null,
                createdCell: function (nTd, sData, oData, iRow) {
                    var startnum = this.api().page() * (this.api().page.info().length);
                    $(nTd).html(iRow + 1 + startnum);
                },
                "sortable": false
            },
            { "data": "Account" },
            { "data": "ProfessionName" },
            { "data": "Id" }
        ],
        "columnDefs": [
            {
                "targets": [2],
                "data": "ProfessionName",
                "render": function (data, type, full) {

                    if (data == undefined || data == "") {
                        return "暂未填写";
                    }
                    else {
                        return data;
                    }
                }
            },
            {
                "targets": [3],
                "data": "Id",
                "render": function (data, type, full) {
                    var str = "";
                    str += "<a onclick=\"deleteUser(" + data + ")\" href=\"javascript:void(0);\" class=\"btn-link\">删除</a>  ";
                    return str;
                }
            }
        ]
    });
}


/**
 * 删除会员
 * @param {any} id 待删除的编号
 */
function deleteUser(id) {
    layer.confirm("您确定要删除吗？", { icon: 3, title: '提示' }, function (index) {
        $.httpPost("/Admin/User/Delete", { id: id }).then(function (req) {
            if (req) {
                alert("删除成功！");
                initTable();//初始化表格
            }
            else {
                alert("啊哦！删除操作失败了~");
            }
            layer.close(index);
        });
    });
}



