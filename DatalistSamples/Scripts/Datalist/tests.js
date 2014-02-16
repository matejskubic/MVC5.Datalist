test('Init options', 11, function() {
    var datalistInput = $('#DatalistInitOptions');
    datalistInput.attr('data-datalist-url', 'http://localhost:9140/Test');
    datalistInput.attr('data-datalist-dialog-title', 'TestTitle');
    datalistInput.attr('data-datalist-sort-column', 'TestColumn');
    datalistInput.attr('data-datalist-hidden-input', 'InitOptions');
    datalistInput.attr('data-datalist-records-per-page', 30);
    datalistInput.attr('data-datalist-sort-order', 'Desc');
    datalistInput.attr('data-datalist-filters', 'A,B,C');

    datalistInput.datalist();

    ok(datalistInput.datalist('option', 'page') == 0);
    ok(datalistInput.datalist('option', 'term') == '');
    ok(datalistInput.datalist('option', 'select') == null);
    ok(datalistInput.datalist('option', 'sortOrder') == 'Desc');
    ok(datalistInput.datalist('option', 'title') == 'TestTitle');
    ok(datalistInput.datalist('option', 'filterChange') == null);
    ok(datalistInput.datalist('option', 'recordsPerPage') == 30);
    ok(datalistInput.datalist('option', 'sortColumn') == 'TestColumn');
    ok(datalistInput.datalist('option', 'url') == 'http://localhost:9140/Test');
    ok(datalistInput.datalist('option', 'filters').join() == ['A', 'B', 'C'].join());
    ok(datalistInput.datalist('option', 'hiddenElement') == document.getElementById('InitOptions'));
});

test('Init options limit records per page', 3, function () {
    var datalistInput = $('#DatalistInitOptionsLimit');
    datalistInput.attr('data-datalist-records-per-page', 'NaN');
    datalistInput.datalist();

    ok(datalistInput.datalist('option', 'recordsPerPage') == 20);

    datalistInput.datalist('destroy');
    datalistInput.attr('data-datalist-records-per-page', -1);
    datalistInput.datalist();

    ok(datalistInput.datalist('option', 'recordsPerPage') == 1);

    datalistInput.datalist('destroy');
    datalistInput.attr('data-datalist-records-per-page', 100);
    datalistInput.datalist();

    ok(datalistInput.datalist('option', 'recordsPerPage') == 99);
});

test('Binds change event on additional filters', 2, function () {
    $('#DatalistFilters').attr('data-datalist-filters', 'Filter1,Filter2');
    $('#DatalistFilters').datalist({
        filterChange: function () {
            ok(true);
        }
    });

    $('#Filter1').change();
    $('#Filter2').change();
});

test('Creates autocomplete', 1, function () {
    ok($('#DatalistAutocomplete').datalist().hasClass('ui-autocomplete-input'));
});

test('Binds key up on autocomplete', 1, function () {
    $('#DatalistAutocompleteKeyup').datalist({
        select: function (e, element, hiddenElement, data) {
            ok(true);
        }
    });

    $('#DatalistAutocompleteKeyup').keyup();
});

test('Removes all preceding elements', 1, function () {
    ok($('#DatalistAutocompleteRemoves').datalist().prevAll().length == 0);
});

asyncTest('Does not call select on load', 0, function () {
    $('#DatalistOnLoadNoSelect').datalist({
        select: function (e, element, hiddenElement, data) {
            ok(false);
        }
    });

    setTimeout(function () {
        start();
    }, 100);
});

asyncTest('Calls select on load', 1, function () {
    $('#DatalistOnLoadSelect').datalist({
        select: function (e, element, hiddenElement, data) {
            ok(true);
        }
    });

    setTimeout(function () {
        start();
    }, 100);
});

test('Cleans up datalist input', 7, function () {
    var datalistInput = $('#DatalistCleanUp').datalist();
    ok(datalistInput.attr('data-datalist-records-per-page') == null);
    ok(datalistInput.attr('data-datalist-dialog-title') == null);
    ok(datalistInput.attr('data-datalist-hidden-input') == null);
    ok(datalistInput.attr('data-datalist-sort-column') == null);
    ok(datalistInput.attr('data-datalist-sort-order') == null);
    ok(datalistInput.attr('data-datalist-filters') == null);
    ok(datalistInput.attr('data-datalist-url') == null);
});

test('Destroys datalist', function () {
    var datalistInput = $('#DatalistDestroy');
    datalistInput.attr('data-datalist-url', 'http://localhost:9140/Test');
    datalistInput.attr('data-datalist-dialog-title', 'TestTitle');
    datalistInput.attr('data-datalist-sort-column', 'TestColumn');
    datalistInput.attr('data-datalist-hidden-input', 'InitOptions');
    datalistInput.attr('data-datalist-records-per-page', 30);
    datalistInput.attr('data-datalist-sort-order', 'Desc');
    datalistInput.attr('data-datalist-filters', 'A,B,C');

    datalistInput.datalist();
    datalistInput.datalist('destroy');
    ok(datalistInput.attr('data-datalist-filters') == 'A,B,C');
    ok(datalistInput.attr('data-datalist-sort-order') == 'Desc');
    ok(datalistInput.attr('data-datalist-records-per-page') == 30);
    ok(datalistInput.attr('data-datalist-sort-column') == 'TestColumn');
    ok(datalistInput.attr('data-datalist-dialog-title') == 'TestTitle');
    ok(datalistInput.attr('data-datalist-hidden-input') == 'InitOptions');
    ok(datalistInput.attr('data-datalist-url') == 'http://localhost:9140/Test');
});