$.province = function (id) {
    $.ajax({
        url: "/commondata/listprovince",
        type: "get",
        dataType: "json",
        data:{isInter:$("#isInter").val()},
        success: function (json) {
            var html = '<option value=0>省</option>';
            if (json.data != null && json.data.length > 0) {
                $.each(json.data, function (k, val) {
                    if (id == val.Id) {
                        html += '<option value=' + val.Id + ' selected>' + val.Name + '</option>';
                    } else {
                        html += '<option value=' + val.Id + '>' + val.Name + '</option>';
                    }
                });
            }
            $("#provinceId").html(html);
        }
    });
}

$.city = function (id, provinceId) {
    $("#cityId").html("");
    var province = $("#provinceId").val();
    if (provinceId > 0) {
        province = provinceId;
    } else {
        if (province == "" || province == null || province == "undefined" || province == "0") {
            $("#cityId").html("<option value=0>市</option");
            return;
        }
    }
    $.ajax({
        url: "/commondata/listcity",
        type: "get",
        dataType: "json",
        data: { provinceId: province },
        success: function (json) {
            var html = '<option value=0>市</option>';
            if (json.data != null && json.data.length > 0) {
                $.each(json.data, function (k, val) {
                    if (id == val.Id) {
                        html += '<option value=' + val.Id + ' selected>' + val.Name + '</option>';
                    } else {
                        html += '<option value=' + val.Id + '>' + val.Name + '</option>';
                    }
                });
            }
            $("#cityId").html(html);
        }
    });
}
