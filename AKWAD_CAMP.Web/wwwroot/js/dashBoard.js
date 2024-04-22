function DrawBarChart(data, divId, cat, val,catText,ValText) {


    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end
    am4core.addLicense("ch-custom-attribution");

    var chart = am4core.create(divId, am4charts.XYChart);
    chart.hiddenState.properties.opacity = 0; // this creates initial fade-in

    chart.data = data;
    chart.cursor = new am4charts.XYCursor();

    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.dataFields.category = cat;
    categoryAxis.renderer.minGridDistance = 40;
    categoryAxis.fontSize = 11;
    categoryAxis.renderer.labels.template.dy = 5;
    categoryAxis.renderer.labels.template.rotation = -45;
    categoryAxis.renderer.labels.template.fontSize = 20;;

    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis.min = 0;
    valueAxis.renderer.minGridDistance = 30;
    valueAxis.renderer.baseGrid.disabled = true;

    chart.rtl = cultureInfo=="ar-EG";

    var series = chart.series.push(new am4charts.ColumnSeries());
    series.dataFields.categoryX = cat;
    series.dataFields.valueY = val;
    series.columns.template.tooltipText = "{valueY.value}";
    series.columns.template.tooltipY = 0;
    series.columns.template.strokeOpacity = 0;


    // as by default columns of the same series are of the same color, we add adapter which takes colors from chart.colors color set
    series.columns.template.adapter.add("fill", function (fill, target) {
        return chart.colors.getIndex(target.dataItem.index);
    });

}

async function postData(url = '') {
    // Default options are marked with *
    const response = await fetch(url, {
        method: 'GET', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
            'Content-Type': 'application/json'
            // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer'// body data type must match "Content-Type" header
    });
    return response.json(); // parses JSON response into native JavaScript objects
}

window.onload = async function () {
    var ChartData = await postData("/Home/ChartsData");

    DrawBarChart(ChartData.top10InTemp,"topInTemp","item","count","Items","Count");
    DrawBarChart(ChartData.top10Expensive, "topExpensive", "item", "price", "Items", "Price");
}

$(document).ready(function () {
    $.getJSON("/Home/GetProductCount", function (data) {

        $('#Products').text(data);

    });
});
$(document).ready(function () {
    $.getJSON("/Home/GetUnitsCount", function (data) {

        $('#Units').text(data);

    });
});
$(document).ready(function () {
    $.getJSON("/Home/GetTemplatesCount", function (data) {

        $('#Templates').text(data);

    });
});
$(document).ready(function () {
    $.getJSON("/Home/GetItemsCount", function (data) {

        $('#Items').text(data);

    });
});