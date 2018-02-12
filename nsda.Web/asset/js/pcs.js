$.province = function (id) {
    $.ajax({
        url: "/commondata/listprovince",
        type: "get",
        dataType: "json",
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
        data:{provinceId:province},
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

$.school = function (id,cityId) {
    $("#schoolId").html("");
    var city = $("#cityId").val();
    if (cityId > 0) {
        city = cityId;
    }
    else {
        if (city == "" || city == null || city == "undefined" || city == "0") {
            $("#schoolId").html("<option value=0>学校</option");
            return;
        }
    }
    $.ajax({
        url: "/commondata/listschool",
        type: "get",
        dataType: "json",
        data: { cityId: city },
        success: function (json) {
            var html = '<option value=0>学校</option>';
            if (json.data != null && json.data.length > 0) {
                $.each(json.data, function (k, val) {
                    if (id == val.Id) {
                        html += '<option value=' + val.Id + ' selected>' + val.Name + '</option>';
                    } else {
                        html += '<option value=' + val.Id + '>' + val.Name + '</option>';
                    }
                });
            }
            $("#schoolId").html(html);
        }
    });
}

$.province1 = function (id) {
    $.ajax({
        url: "/commondata/listprovince",
        type: "get",
        dataType: "json",
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
            $("#provinceId1").html(html);
        }
    });
}

$.city1 = function (id, provinceId) {
    $("#cityId1").html("");
    var province = $("#provinceId1").val();
    if (provinceId > 0) {
        province = provinceId;
    } else {
        if (province == "" || province == null || province == "undefined" || province == "0") {
            $("#cityId1").html("<option value=0>市</option");
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
            $("#cityId1").html(html);
        }
    });
}

$.school1 = function (id, cityId) {
    $("#schoolId1").html("");
    var city = $("#cityId1").val();
    if (cityId > 0) {
        city = cityId;
    }
    else {
        if (city == "" || city == null || city == "undefined" || city == "0") {
            $("#schoolId1").html("<option value=0>学校</option");
            return;
        }
    }
    $.ajax({
        url: "/commondata/listschool",
        type: "get",
        dataType: "json",
        data: { cityId: city },
        success: function (json) {
            var html = '<option value=0>学校</option>';
            if (json.data != null && json.data.length > 0) {
                $.each(json.data, function (k, val) {
                    if (id == val.Id) {
                        html += '<option value=' + val.Id + ' selected>' + val.Name + '</option>';
                    } else {
                        html += '<option value=' + val.Id + '>' + val.Name + '</option>';
                    }
                });
            }
            $("#schoolId1").html(html);
        }
    });
}

