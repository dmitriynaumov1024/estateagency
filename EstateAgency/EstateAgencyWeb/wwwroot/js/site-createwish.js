function gettowns(region) {
    $.ajax({
        type: "GET",
        url: `/ajax/gettownsofregion?region=${region}`,
        data: "",
        success: function (data) {
            $("#districtselect").html("");
            if (data.length == 0) {
                $("#townselect").html("");
                return;
            }
            let s = `<option value='0'>-- Не вибрано --</option>`;
            for (let i = 0; i < data.length; i++) {
                s += `<option value='${data[i]}'>${data[i]}</option>`;
            }
            $("#townselect").html(s);
            s = "";
        }
    });
}

function getdistricts(town) {
    $.ajax({
        type: "GET",
        url: `/ajax/getdistrictsoftown?town=${town}`,
        data: "",
        success: function (data) {
            let s = "";
            let i = 0;
            for (var key in data) {
                s += `<option value='${key}'>${data[key]}</option>`;
                i++;
            }
            $("#districtselect").html(s);
            if (i == 0) $("#districtselect").html("");
            s = "";
        }
    });
}

function page_main() {
    $("#regionselect").change(function () { gettowns($("#regionselect").val()) });
    $("#townselect").change(function () { getdistricts($("#townselect").val()) });
}
