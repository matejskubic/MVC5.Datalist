var testInput, hiddenInput, openSpan, filter1, filter2;

QUnit.testStart(function (details) {
    openSpan = $('#test-data > .input-group > .datalist-open-span');
    testInput = $('#TestDatalist');
    hiddenInput = $('#Test');
    filter1 = $('#Filter1');
    filter2 = $('#Filter2');

    testInput
        .attr('data-datalist-url', 'http://localhost:9140/Datalist/Default')
        .attr('data-datalist-filters', 'Filter1,Filter2')
        .attr('data-datalist-dialog-title', 'TestTitle')
        .attr('data-datalist-sort-column', 'FirstName')
        .attr('class', 'form-control datalist-input')
        .attr('data-datalist-records-per-page', 30)
        .attr('data-datalist-hidden-input', 'Test')
        .attr('data-datalist-sort-order', 'Desc')
        .attr('data-datalist-term', 'test')
        .attr('data-datalist-page', '1');
});
QUnit.testDone(function (details) {
    testInput.val('').clone().appendTo('#test-data > .input-group');
    testInput.remove();

    hiddenInput.val('').clone().appendTo('#test-data > .input-group');
    hiddenInput.remove();
    
    openSpan.clone().appendTo('#test-data > .input-group');
    openSpan.remove();
    
    filter1.val('').clone().appendTo('#test-data');
    filter1.remove();

    filter2.val('').clone().appendTo('#test-data');
    filter2.remove();
});

test('Does not create mvc-datalist on non datalist input', 1, function () {
    equal(filter1.datalist().hasClass('mvc-datalist'), false);
});

test('Initializes options', 12, function () {
    testInput.datalist();

    ok(testInput.hasClass('mvc-datalist'));
    equal(testInput.datalist('option', 'page'), 1);
    equal(testInput.datalist('option', 'term'), 'test');
    equal(testInput.datalist('option', 'select'), null);
    equal(testInput.datalist('option', 'sortOrder'), 'Desc');
    equal(testInput.datalist('option', 'title'), 'TestTitle');
    equal(testInput.datalist('option', 'filterChange'), null);
    equal(testInput.datalist('option', 'recordsPerPage'), 30);
    equal(testInput.datalist('option', 'sortColumn'), 'FirstName');
    equal(testInput.datalist('option', 'hiddenElement'), hiddenInput[0]);
    equal(testInput.datalist('option', 'url'), 'http://localhost:9140/Datalist/Default');
    equal(testInput.datalist('option', 'filters').join(), ['Filter1', 'Filter2'].join());
});

test('Initializes filters', 2, function () {
    testInput.datalist().data('mvc-datalist')._select = function (data) {
        equal(data, null);
    };

    filter1.change();
    filter2.change();
});
test('Initializes filters with events', 10, function () {
    var filters = [filter1, filter2];
    var iteration = 0;

    testInput.datalist({
        filterChange: function (e, element, hiddenElement, filter) {
            equal(filter, filters[iteration++][0]);
            equal(hiddenElement, hiddenInput[0]);
            equal(element, testInput[0]);
            ok(e);
        }
    });

    testInput.data('mvc-datalist')._select = function (data) {
        equal(data, null);
    };

    filters[0].change();
    filters[1].change();
});
test('Initializes custom filter change', 0, function () {
    testInput.datalist({
        filterChange: function (e, element, hiddenElement, fitler) {
            e.preventDefault();
        }
    });

    testInput.data('mvc-datalist')._select = function (data) {
        ok(false);
    };

    filter1.change();
    filter2.change();
});
// TODO: Somehow test autocomplete source method
test('Initializes autocomplete', 2, function () {
    ok(testInput.datalist().hasClass('ui-autocomplete-input'));
    equal(testInput.autocomplete('option', 'minLength'), 1);
});
test('Initializes autocomplete select', 2, function () {
    testInput.datalist().data('mvc-datalist')._select = function (data) {
        equal(data.DatalistIdKey, 'Test2');
        equal(data.DatalistAcKey, 'Test3');
    };
    
    testInput.data('ui-autocomplete')._trigger('select', 'autocompleteselect', {
            item: {
                item: {
                    DatalistIdKey: 'Test2',
                    DatalistAcKey: 'Test3'
                }
            }
        });
});
test('Initializes keyup on autocomplete', 1, function () {
    testInput.datalist().data('mvc-datalist')._select = function (data) {
        equal(data, null);
    };

    testInput.keyup();
});
test('Removes all preceding elements', 1, function () {
    equal(testInput.datalist().prevAll().length, 0);
});

test('Initializes datalist open span', 16, function () {
    testInput.datalist().data('mvc-datalist')._update = function (datalist) {
        equal(datalist[0], $('#Datalist')[0]);
    };
    testInput.data('mvc-datalist')._limitTo = function (value, min, max) {
        equal(value, 30);
        equal(max, 99);
        equal(min, 1);
        return value;
    };

    openSpan.click();
    $('#Datalist').dialog('close');

    testInput.data('mvc-datalist')._limitTo = function (value, min, max) {
        equal(value, 2);
        equal(max, 99);
        equal(min, 1);
        return value;
    };

    $('.datalist-items-per-page').val(2).data('ui-spinner')._trigger('change', 'spinnerchange');
    equal($('#Datalist').dialog('option', 'title'), testInput.datalist('option', 'title'));
    equal($('.datalist-search-input').attr('placeholder'), $.fn.datalist.lang.Search);
    equal($('.datalist-error-span').html(), $.fn.datalist.lang.Error);
    equal(testInput.datalist('option', 'recordsPerPage'), 2);
    equal(testInput.datalist('option', 'page'), 0);

    $('.datalist-search-input').val('test2').keyup();
    stop();
    setTimeout(function () {
        start();
        equal(testInput.datalist('option', 'term'), 'test2');
        equal(testInput.datalist('option', 'page'), 0);
    }, 500);
});

test('Forms autocomplete url', 1, function () {
    testInput.attr('data-datalist-filters', '').datalist();
    var expected = testInput.datalist('option', 'url') +
        '?SearchTerm=test' +
        '&RecordsPerPage=20' +
        '&SortOrder=Asc' +
        '&Page=0';

    equal(testInput.data('mvc-datalist')._formAutocompleteUrl('test'), expected);
});
test('Forms autocomplete url with filters', 1, function () {
    filter1.val('Filter1');
    filter2.val('Filter2');
    testInput.datalist();

    var expected = testInput.datalist('option', 'url') +
        '?SearchTerm=test' +
        '&RecordsPerPage=20' +
        '&SortOrder=Asc' +
        '&Page=0' +
        '&Filter1=Filter1' +
        '&Filter2=Filter2';

    equal(testInput.data('mvc-datalist')._formAutocompleteUrl('test'), expected);
});

test('Forms datalist url', 1, function () {
    testInput.attr('data-datalist-filters', '').datalist();
    var expected = testInput.datalist('option', 'url') +
        '?SearchTerm=test' +
        '&RecordsPerPage=' + testInput.datalist('option', 'recordsPerPage') +
        '&SortColumn=' + testInput.datalist('option', 'sortColumn') +
        '&SortOrder=' + testInput.datalist('option', 'sortOrder') +
        '&Page=' + testInput.datalist('option', 'page');

    equal(testInput.data('mvc-datalist')._formDatalistUrl('test'), expected);
});
test('Forms datalist url with filters', 1, function () {
    filter1.val('Filter1');
    filter2.val('Filter2');
    testInput.datalist();

    var expected = testInput.datalist('option', 'url') +
        '?SearchTerm=test' +
        '&RecordsPerPage=' + testInput.datalist('option', 'recordsPerPage') +
        '&SortColumn=' + testInput.datalist('option', 'sortColumn') +
        '&SortOrder=' + testInput.datalist('option', 'sortOrder') +
        '&Page=' + testInput.datalist('option', 'page') +
        '&Filter1=Filter1' +
        '&Filter2=Filter2';

    equal(testInput.data('mvc-datalist')._formDatalistUrl('test'), expected);
});

test('Forms empty additional filter query', 1, function () {
    testInput.attr('data-datalist-filters', '').datalist();
    equal(testInput.data('mvc-datalist')._formFiltersQuery(), '');
});
test('Forms additional filter query', 1, function () {
    filter1.val('Test1');
    filter2.val('Test2');
    testInput.datalist();
    equal(testInput.data('mvc-datalist')._formFiltersQuery(), '&Filter1=Test1&Filter2=Test2');
});

test('Default selects', 2, function () {
    data = { DatalistIdKey: 'Test1', DatalistAcKey: 'Test2' };
    testInput.datalist().data('mvc-datalist')._defaultSelect(data);

    equal(hiddenInput.val(), data.DatalistIdKey);
    equal(testInput.val(), data.DatalistAcKey);
});
test('Default select clears values', 2, function () {
    testInput.datalist();
    hiddenInput.val(1);
    testInput.val(1);

    testInput.datalist().data('mvc-datalist')._defaultSelect(null);

    equal(hiddenInput.val(), '');
    equal(testInput.val(), '');
});

asyncTest('Does not call select on load', 0, function () {
    testInput.datalist({
        select: function () {
            ok(false);
        }
    });

    setTimeout(function () {
        start();
    }, 200);

    testInput.datalist('destroy');

    stop();
    hiddenInput.val(0);
    testInput.datalist({
        select: function () {
            ok(false);
        }
    });

    setTimeout(function () {
        start();
    }, 200);
});
asyncTest('Calls select on load', 1, function () {
    hiddenInput.val(1);
    testInput.datalist({
        select: function () {
            ok(true);
        }
    });

    setTimeout(function () {
        start();
    }, 200);
});

test('Calls select and default select', 6, function () {
    var selectedData = { DatalistIdKey: 'Test' };
    testInput.datalist({
        select: function (e, element, hiddenElement, data) {
            equal(hiddenElement, hiddenInput[0]);
            equal(element, testInput[0]);
            equal(data, selectedData);
            ok(e);
        }
    });

    testInput.data('mvc-datalist')._defaultSelect = function (data) {
        equal(data, selectedData);
    };

    testInput.data('mvc-datalist')._select(selectedData);

    testInput.datalist('destroy').datalist();
    testInput.data('mvc-datalist')._defaultSelect = function (data) {
        equal(data, selectedData);
    };

    testInput.data('mvc-datalist')._select(selectedData);
});
test('Default select prevented', 0, function () {
    testInput.datalist({
        select: function (e) {
            e.preventDefault();
        }
    });

    testInput.data('mvc-datalist')._defaultSelect = function () {
        ok(false);
    };

    testInput.data('mvc-datalist')._select();
});

test('Limits value', 6, function () {
    testInput.datalist();
    var datalist = testInput.data('mvc-datalist');

    equal(datalist._limitTo('NotNumber', 1, 99), 20);
    equal(datalist._limitTo(-1, 1, 99), 1);
    equal(datalist._limitTo(1, 1, 99), 1);
    equal(datalist._limitTo(100, 1, 99), 99);
    equal(datalist._limitTo(99, 1, 99), 99);
    equal(datalist._limitTo(60, 1, 99), 60);
});

test('Cleans up', 9, function () {
    testInput.datalist();
    equal(testInput.attr('data-datalist-records-per-page'), null);
    equal(testInput.attr('data-datalist-dialog-title'), null);
    equal(testInput.attr('data-datalist-hidden-input'), null);
    equal(testInput.attr('data-datalist-sort-column'), null);
    equal(testInput.attr('data-datalist-sort-order'), null);
    equal(testInput.attr('data-datalist-filters'), null);
    equal(testInput.attr('data-datalist-term'), null);
    equal(testInput.attr('data-datalist-page'), null);
    equal(testInput.attr('data-datalist-url'), null);
});

test('Destroys datalist', 10, function () {
    testInput.datalist({
        filterChange: function () {
            ok(false);
        }
    });

    testInput.datalist('destroy');
    equal(testInput.attr('data-datalist-sort-order'), 'Desc');
    equal(testInput.attr('data-datalist-hidden-input'), 'Test');
    equal(testInput.attr('data-datalist-records-per-page'), 30);
    equal(testInput.attr('data-datalist-sort-column'), 'FirstName');
    equal(testInput.attr('data-datalist-dialog-title'), 'TestTitle');
    equal(testInput.attr('data-datalist-filters'), 'Filter1,Filter2');
    equal(testInput.attr('data-datalist-url'), 'http://localhost:9140/Datalist/Default');

    equal(testInput.hasClass('mvc-datalist'), false);
    equal(testInput.data('ui-autocomplete'), null);
    equal(testInput.data('mvc-datalist'), null);
    filter1.change();
    filter2.change();
});

test('Datalist language init', 3, function () {
    equal($.fn.datalist.lang.Error, 'Error while retrieving records');
    equal($.fn.datalist.lang.NoDataFound, 'No data found');
    equal($.fn.datalist.lang.Search, 'Search...');
});
test('Datalist spinner init', 4, function () {
    var datalistSpinner = $('.datalist-items-per-page');
    ok(datalistSpinner.hasClass('ui-spinner-input'));
    equal(datalistSpinner.spinner('option', 'min'), 1);
    equal(datalistSpinner.spinner('option', 'max'), 99);
    ok(datalistSpinner.parent().hasClass('input-group-addon'));
});
test('Datalist dialog init', 7, function () {
    var datalist = $('#Datalist');
    ok(datalist.hasClass('ui-dialog-content'));
    equal(datalist.dialog('option', 'autoOpen'), false);
    equal(datalist.dialog('option', 'minHeight'), 210);
    equal(datalist.dialog('option', 'height'), 'auto');
    equal(datalist.dialog('option', 'minWidth'), 455);
    equal(datalist.dialog('option', 'width'), 'auto');
    equal(datalist.dialog('option', 'modal'), true);
});