var common = {
    init: function () {
        common.registerEvent();
    },
    registerEvent: function () {
        $("#txtSearchString").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Home/DSphim",
                    dataType: "json",
                    data: {
                        q: request.term
                    },
                    success: function (data) {
                        response(data.data);
                    }
                });
            },
            focus: function (event, ui) {
                $("#txtSearchString").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#txtSearchString").val(ui.item.label);
                return false;
            }
        })
            .autocomplete("instance")._renderItem = function (ul, item) {
                return $("<li>")
                    .append("<a>" + item.label + "</a>")
                    .appendTo(ul);
            };
    }
}
common.init();