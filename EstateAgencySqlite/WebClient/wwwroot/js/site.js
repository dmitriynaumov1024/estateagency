const row_load_count = 100; 
var level2_offset = 0;
var main_backup;
var last_function = undefined;
var changed = false;

function site_main() {
    $("#menu0person").click(function () {
        reset_main();
        person_getall(0, row_load_count);
    });
    $("#menu0location").click(function () {
        reset_main();
        location_getall(0, row_load_count);
    });
    $("#menu0estateobject").click(function () {
        reset_main();
        estateobject_getall(0, row_load_count);
    });
    $("#menu0bookmark").click(function () {
        reset_main();
        bookmark_getall(0, row_load_count);
    });
    $("#menu0clientwish").click(function () {
        reset_main();
        clientwish_getall(0, row_load_count);
    });
    $("#menu0agent").click(function () {
        reset_main();
        agent_getall(0, row_load_count);
    });
    $("#menu0deal").click(function () {
        reset_main();
        deal_getall(0, row_load_count);
    });
    $("#menu0rawsql").click(function () {
        reset_main();
        sql_prepare();
    });
}

function reset_main() {
    level2_offset = 0;
    last_function = undefined;
    changed = false;
    $("#main").html("");
}

const magic_buttons = `
<table style='table-layout:fixed; width:100%; margin-top:6em;'>
<tr>
<td><button id='prev' class='secondary' onclick='if(level2_offset>=row_load_count) {last_function(level2_offset-row_load_count, row_load_count)}'>&lt;&lt;</button></td>
<td>
</td>
<td><button id='next' class='secondary' onclick='last_function(level2_offset+row_load_count, row_load_count)'>&gt;&gt;</button></td>
</tr>
</table>
`;

function magic_add_button(onclickfunctionname) {
    return `<div style='position:fixed; bottom:6px; right:6px; box-shadow:0 0 6px 0 rgba(0.9,0.9,0.9,0.4);'><button id='add' class='primary' onclick='${onclickfunctionname}()'>+ NEW</button></div>`
}

function build_table(data, rowclickfunction) {
    let tablehtml = `<table class="dataview"><tr>`;
    data.Header.forEach((a) => { tablehtml += `<th>${a}</th>` });
    tablehtml += "</tr>";
    for (var i = 0; i < data.Data.length; i++) {
        let rowhtml = `<tr onclick='${rowclickfunction(data.Data[i][0])}'>`;
        data.Data[i].forEach((a) => { rowhtml += `<td>${a}</td>` });
        rowhtml += "</tr>";
        tablehtml += rowhtml;
    }
    return (tablehtml + "</table>");
}

function build_table2(data, rowclickfunction) {
    let tablehtml = `<table class="dataview"><tr>`;
    data.Header.forEach((a) => { tablehtml += `<th>${a}</th>` });
    tablehtml += "<th></th></tr>";
    for (var i = 0; i < data.Data.length; i++) {
        let rowhtml = `<tr>`;
        data.Data[i].forEach((a) => { rowhtml += `<td>${a}</td>` });
        rowhtml += `<td style='color:darkred; cursor:pointer; text-align:center;' onclick='${rowclickfunction(data.Data[i][0], data.Data[i][1])}'><b> X </b></td></tr>`;
        tablehtml += rowhtml;
    }
    return (tablehtml + "</table>");
}

function build_table3(data, rowclickfunction) {
    let tablehtml = `<table class="dataview"><tr>`;
    data.Header.forEach((a) => { tablehtml += `<th>${a}</th>` });
    tablehtml += "<th></th></tr>";
    for (var i = 0; i < data.Data.length; i++) {
        let rowhtml = `<tr>`;
        data.Data[i].forEach((a) => { rowhtml += `<td>${a}</td>` });
        rowhtml += `<td style='color:darkred; cursor:pointer; text-align:center;' onclick='${rowclickfunction(data.Data[i][0])}'><b> X </b></td></tr>`;
        tablehtml += rowhtml;
    }
    return (tablehtml + "</table>");
}

function build_table_simple(data) {
    let tablehtml = `<table class="dataview"><tr>`;
    data.Header.forEach((a) => { tablehtml += `<th>${a}</th>` });
    tablehtml += "</tr>";
    for (var i = 0; i < data.Data.length; i++) {
        let rowhtml = '<tr>';
        data.Data[i].forEach((a) => { rowhtml += `<td>${a}</td>` });
        rowhtml += "</tr>";
        tablehtml += rowhtml;
    }
    return (tablehtml + "</table>");
}

function build_notfound() {
    return "<span style='position:absolute; top:50%; left:50%; transform:translate(-50%,-50%);'>Nothing was found!</span>";
}

function sql_prepare() {
    $("#main").html(`
<div><textarea /></div>`);
}

// It is supposed server will return {Header:[], Data:[]}

// Person
function person_getall(start_index, count){
    $.ajax({
        type: "GET",
        url: `/ajax/person/getall?start_index=${start_index}&count=${count}`,
        success: function (data) {
            if (!data.Header || !data.Header.length || !data.Data.length) {
                if (level2_offset == 0)
                    $("#main").html(build_notfound());
                $('#next').hide();
                $("#main").append(magic_add_button('person_add'));
                return;
            }
            $("#main").html(build_table(data, (i) => `person_get(${i})`));
            last_function = person_getall;
            level2_offset = start_index;
            $("#main").append(magic_buttons);
            $("#main").append(magic_add_button('person_add'));
        }
    });
}

function person_get(id) {
    $.ajax({
        type: "GET",
        url: `/ajax/person/get?id=${id}`,
        success: function (data) {
            if (data.Found) {
                main_backup = $("#main").html();
                $("#main").html(data.Html);
            }
        }
    });
}

function person_add() {
    $.ajax({
        type: "GET",
        url: '/ajax/person/add',
        success: function (data) {
            main_backup = $("#main").html();
            $("#main").html(data.Html);
        }
    })
}
// ------------------------------------------------------------

// Locations
function location_getall(start_index, count) {
    $.ajax({
        type: "GET",
        url: `/ajax/location/getall?start_index=${start_index}&count=${count}`,
        success: function (data) {
            if (!data.Header || !data.Header.length || !data.Data.length) {
                if (level2_offset == 0)
                    $("#main").html(build_notfound());
                $('#next').hide();
                $("#main").append(magic_add_button('location_add'));
                return;
            }
            $("#main").html(build_table(data, (i) => `location_get(${i})`));
            last_function = location_getall;
            level2_offset = start_index;
            $("#main").append(magic_buttons);
            $("#main").append(magic_add_button('location_add'));
        }
    });
}

function location_get(id) {
    $.ajax({
        type: "GET",
        url: `/ajax/location/get?id=${id}`,
        success: function (data) {
            if (data.Found) {
                main_backup = $("#main").html();
                $("#main").html(data.Html);
            }
        }
    });
}

function location_add() {
    $.ajax({
        type: "GET",
        url: '/ajax/location/add',
        success: function (data) {
            main_backup = $("#main").html();
            $("#main").html(data.Html);
        }
    })
}
// ------------------------------------------------------------

// EstateObject
function estateobject_getall(start_index, count) {
    $.ajax({
        type: "GET",
        url: `/ajax/estateobject/getall?start_index=${start_index}&count=${count}`,
        success: function (data) {
            if (!data.Header || !data.Header.length || !data.Data.length) {
                if (level2_offset == 0)
                    $("#main").html(build_notfound());
                $('#next').hide();
                $("#main").append(magic_add_button('estateobject_add'));
                return;
            }
            $("#main").html(build_table(data, (i) => `estateobject_get(${i})`));
            last_function = estateobject_getall;
            level2_offset = start_index;
            $("#main").append(magic_buttons);
            $("#main").append(magic_add_button('estateobject_add'));
        }
    });
}

function estateobject_get(id) {
    $.ajax({
        type: "GET",
        url: `/ajax/estateobject/get?id=${id}`,
        success: function (data) {
            if (data.Found) {
                main_backup = $("#main").html();
                $("#main").html(data.Html);
            }
        }
    });
}

function estateobject_add() {
    $.ajax({
        type: "GET",
        url: '/ajax/estateobject/add',
        success: function (data) {
            main_backup = $("#main").html();
            $("#main").html(data.Html);
        }
    })
}
// --------------------------------------------------------

// Agent
function agent_getall(start_index, count) {
    $.ajax({
        type: "GET",
        url: `/ajax/agent/getall?start_index=${start_index}&count=${count}`,
        success: function (data) {
            if (!data.Header || !data.Header.length || !data.Data.length) {
                if (level2_offset == 0)
                    $("#main").html(build_notfound());
                $('#next').hide();
                $("#main").append(magic_add_button('agent_add'));
                return;
            }
            $("#main").html(build_table(data, (i) => `agent_get(${i})`));
            last_function = agent_getall;
            level2_offset = start_index;
            $("#main").append(magic_buttons);
            $("#main").append(magic_add_button('agent_add'));
        }
    });
}

function agent_get(id) {
    $.ajax({
        type: "GET",
        url: `/ajax/agent/get?id=${id}`,
        success: function (data) {
            if (data.Found) {
                main_backup = $("#main").html();
                $("#main").html(data.Html);
            }
        }
    });
}

function agent_add() {
    $.ajax({
        type: "GET",
        url: '/ajax/agent/add',
        success: function (data) {
            if (!data.Good) return;
            main_backup = $("#main").html();
            $("#main").html(data.Html);
        }
    })
}
// -----------------------------------------------

// ClientWish
function clientwish_getall(start_index, count) {
    $.ajax({
        type: "GET",
        url: `/ajax/clientwish/getall?start_index=${start_index}&count=${count}`,
        success: function (data) {
            if (!data.Header || !data.Header.length || !data.Data.length) {
                if (level2_offset == 0)
                    $("#main").html(build_notfound());
                $('#next').hide();
                $("#main").append(magic_add_button('clientwish_add'));
                return;
            }
            $("#main").html(build_table(data, (i) => `clientwish_get(${i})`));
            last_function = clientwish_getall;
            level2_offset = start_index;
            $("#main").append(magic_buttons);
            $("#main").append(magic_add_button('clientwish_add'));
        }
    });
}

function clientwish_get(id) {
    $.ajax({
        type: "GET",
        url: `/ajax/clientwish/get?id=${id}`,
        success: function (data) {
            if (data.Found) {
                main_backup = $("#main").html();
                $("#main").html(data.Html);
            }
        }
    });
}

function clientwish_add() {
    $.ajax({
        type: "GET",
        url: '/ajax/clientwish/add',
        success: function (data) {
            if (!data.Good) return;
            main_backup = $("#main").html();
            $("#main").html(data.Html);
        }
    })
}
// -------------------------------------------------------------------

// Bookmark
function bookmark_getall(start_index, count) {
    $.ajax({
        type: "GET",
        url: `/ajax/bookmark/getall?start_index=${start_index}&count=${count}`,
        success: function (data) {
            if (!data.Header || !data.Header.length || !data.Data.length) {
                if (level2_offset == 0)
                    $("#main").html(build_notfound());
                $('#next').hide();
                $("#main").append(magic_add_button('bookmark_add'));
                return;
            }
            $("#main").html(build_table2(data, (i, j) => `bookmark_delete(${i},${j})`));
            last_function = bookmark_getall;
            level2_offset = start_index;
            $("#main").append(magic_buttons);
            $("#main").append(magic_add_button('bookmark_add'));
        }
    });
}

function bookmark_delete(personid, objectid) {
    $.ajax({
        type: "DELETE",
        url: `/ajax/bookmark/delete?personid=${personid}&objectid=${objectid}`,
        success: function (data) {
            if (data) {
                $("#main tr").toArray().forEach((a) => { if (a.children[0].innerHTML==personid && a.children[1].innerHTML==objectid) a.hidden = true })
            }
        }
    });
}

function bookmark_add() {
    $.ajax({
        type: "GET",
        url: '/ajax/bookmark/add',
        success: function (data) {
            if (!data.Good) return;
            main_backup = $("#main").html();
            $("#main").html(data.Html);
        }
    })
}
// -------------------------------------------------------------------

// Deal
function deal_getall(start_index, count) {
    $.ajax({
        type: "GET",
        url: `/ajax/deal/getall?start_index=${start_index}&count=${count}`,
        success: function (data) {
            if (!data.Header || !data.Header.length || !data.Data.length) {
                if (level2_offset == 0)
                    $("#main").html(build_notfound());
                $('#next').hide();
                $("#main").append(magic_add_button('deal_add'));
                return;
            }
            $("#main").html(build_table3(data, (i) => `deal_delete(${i})`));
            last_function = deal_getall;
            level2_offset = start_index;
            $("#main").append(magic_buttons);
            $("#main").append(magic_add_button('deal_add'));
        }
    });
}

function deal_delete(id) {
    $.ajax({
        type: "DELETE",
        url: `/ajax/deal/delete?id=${id}`,
        success: function (data) {
            if (data) {
                $("#main tr").toArray().forEach((a) => { if (a.firstElementChild.innerHTML == id) a.hidden = true })
            }
        }
    });
}

function deal_add() {
    $.ajax({
        type: "GET",
        url: '/ajax/deal/add',
        success: function (data) {
            if (!data.Good) return;
            main_backup = $("#main").html();
            $("#main").html(data.Html);
        }
    })
}
//-------------------------------------------------

// RAW SQL, FINALLY
function sql_prepare() {
    $("#main").html(`<h2>Raw SQL<br/></h2>
<div style='width:100%'>
Enter your query here
<br/>
<textarea id='Query' style='background-color: #f7f7f7; width:60%; font-family:monospace !important; height:9em; maxlength:1000;' placeholder='select impostor from among us limit 1...'></textarea>
<button class='primary' style='width:8em;' onclick='sql_fire()'> Execute! </button>
</div>
<br/>
<br/>
<div>
Result of your query
<br/>
</div>
<div id='rawsqloutput'>
</div>
`);
}

function sql_fire() {
    $.ajax({
        type: "POST",
        url: '/ajax/rawsql',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        contentType: 'application/json', dataType: 'json',
        data: JSON.stringify({
            'Query': $('#Query').val()
        }),
        success: function (data) {
            main_backup = undefined;
            if (!data.Good) {
                $("#rawsqloutput").html("Exception message: " + data.Message);
                return;
            }
            $("#rawsqloutput").html(build_table_simple(data));
        }
    });
}