$('tr.schematic-row').click(function () {
    $('.selected-schematic').removeClass('selected-schematic');
    $(this).addClass('selected-schematic');

    var matCells = $(this).find('td span').parent();
    $('.required-mat').removeClass('required-mat');
    $('.required-planet').removeClass('required-planet');

    matCells.addClass('required-mat');

    matCells.each(function (index, item) {
        var dataColumn = $(item).attr('data-column');
        var requiredMats = $('tr.planet-row td.mat-box[data-column="' + dataColumn + '"] span').parent();
        var requiredPlanets = requiredMats.parent('tr.planet-row');
        var matHeaders = $('th.raw-mat-header[data-column="' + dataColumn + '"] div');

        requiredMats.addClass('required-mat');
        requiredPlanets.addClass('required-planet');
        matHeaders.addClass('required-mat');
    });
});

$('tr.schematic-header-row th.sortable').click(function () {
    var dataColumn = $(this).attr('data-column'); //get the column name
    var dataSort = $(this).attr('data-sort'); //get the sort order
    var rows = $('tr.schematic-row').get(); //get the rows to sort

    // reset column sort orders to in order
    $('tr.schematic-header-row th.sortable').attr('data-sort', 'no-order');

    // sort rows
    rows.sort(function (a, b) {
        var aVal = $(a).find('td[data-column="' + dataColumn + '"],th[data-column="' + dataColumn + '"]').text().toUpperCase();
        var bVal = $(b).find('td[data-column="' + dataColumn + '"],th[data-column="' + dataColumn + '"]').text().toUpperCase();

        if ($.isNumeric(aVal.replace(/,/g, '')) && $.isNumeric(bVal.replace(/,/g, ''))) {
            aVal = Number(aVal.replace(/,/g, ''));
            bVal = Number(bVal.replace(/,/g, ''));
        }

        if (dataSort === 'no-order' || dataSort === 'post-order') {
            return SortInOrder(aVal, bVal);
        }
        else {
            return SortPostOrder(aVal, bVal);
        }
    });

    // append rows in specified order
    $.each(rows, function (index, row) {
        $('tbody').append(row);
    });

    // switch sort order for column
    switch (dataSort) {
        case 'no-order':
        case 'post-order':
            $(this).attr('data-sort', 'in-order');
            break;
        case 'in-order':
            $(this).attr('data-sort', 'post-order');
            break;
    }

    sessionStorage.PISortCol = $(this).attr('data-column');
    sessionStorage.PISortOrder = $(this).attr('data-sort');
});

$('button.view-toggle').click(function () {
    var viewToHide = $(this).attr('data-view');

    $('th.hidden').removeClass('hidden');
    $('td.hidden').removeClass('hidden');

    $('th.' + viewToHide).addClass('hidden');
    $('td.' + viewToHide).addClass('hidden');

    switch (viewToHide) {
        case 'build-cost':
            $(this).attr('data-view', 'profit');
            $(this).text('View Build Cost');
            break;
        case 'profit':
            $(this).attr('data-view', 'build-cost');
            $(this).text('View Profit');
            break;
    }

    sessionStorage.PIView = $(this).attr('data-view');
});

$('#TradeHub').change(function () {
    var systemId = this.value;
    var systemName = $(this).find('option[value="' + systemId + '"]').text();
    
    location.href = systemName;
});

$('#market-refresh').click(function () {
    location.reload();
});

function SortInOrder(a, b) {
    if (a < b) {
        return -1;
    }

    if (a > b) {
        return 1;
    }

    return 0;
}

function SortPostOrder(a, b) {
    if (a > b) {
        return -1;
    }

    if (a < b) {
        return 1;
    }

    return 0;
}