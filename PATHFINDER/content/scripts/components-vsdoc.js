/// <reference name="MicrosoftAjax.js"/>
/// <reference path="~/content/scripts/jquery-1.4.1-vsdoc.js" />

Type.registerNamespace("Pathfinder.UI");

Pathfinder.UI.CheckboxList = function(element) {
    Pathfinder.UI.CheckboxList.initializeBase(this, [element]);

    this._items = null;

    this._maxItems = 0;

    this._breakCount = 0;

    this._count = 0;

    this._booleanOptions = false;

    this._optionNameFormat = "opt_{0}";

    this._trueValue = true;

    this._defaultText = "";

    this._multiItemText = null;

    this._itemCssClass = null;

    this._itemClickedDelegate = null;

    this._default = "0"; //forced selection

    this._allOptionID = "0";

}

Pathfinder.UI.CheckboxList.prototype = {
    initialize: function() {
        Pathfinder.UI.CheckboxList.callBaseMethod(this, 'initialize');

        var e = this.get_element();

        $addHandler(e, "click", (this._itemClickedDelegate = Function.createDelegate(this, this._itemClicked)));
    },

    dispose: function() {
        if (this._itemClickedDelegate) {
            try { $removeHandler(this.get_element(), "click", this._itemClickedDelegate); } catch (ex) { }
            delete (this._itemClickedDelegate);
        }
        Pathfinder.UI.CheckboxList.callBaseMethod(this, 'dispose');
    },

    add_itemClicked: function(handler) { this.get_events().addHandler("itemclicked", handler); },
    remove_itemClicked: function(handler) { this.get_events().removeHandler("itemclicked", handler); },

    add_error: function(handler) { this.get_events().addHandler("error", handler); },
    remove_error: function(handler) { this.get_events().removeHandler("error", handler); },

    get_maxItems: function() { return this._maxItems; },
    set_maxItems: function(value) { this._maxItems = value; },

    get_breakCount: function() { return this._breakCount; },
    set_breakCount: function(value) { this._breakCount = value; },

    get_itemCssClass: function() { return this._itemCssClass; },
    set_itemCssClass: function(value) { this._itemCssClass = value; },

    get_booleanOptions: function() { return this._booleanOptions; },
    set_booleanOptions: function(value) { this._booleanOptions = value; },

    get_trueValue: function() { return this._trueValue; },
    set_trueValue: function(value) { this._trueValue = value; },

    get_optionNameFormat: function() { return this._optionNameFormat; },
    set_optionNameFormat: function(value) {
        if (!value) return;
        this._optionNameFormat = value;
    },

    get_defaultValue: function() { return this._default; },
    set_defaultValue: function(value) { this._default = value; },

    get_allOptionID: function() { return this._allOptionID; },
    set_allOptionID: function(value) { this._allOptionID = value; },

    get_multiItemText: function() { return this._multiItemText; },
    set_multiItemText: function(value) { this._multiItemText = value; },

    get_defaultText: function() {
        if (this._defaultText) return this._defaultText;

        return "";
    },
    set_defaultText: function(value) { this._defaultText = value; },

    get_items: function() { return $.makeArray(this._items); },
    set_items: function(value) {
        if ($.isArray(value)) {
            var item;
            for (var i = 0; i < value.length; i++) {
                item = value[i];
                this.addItem(item.id, item.text, item.value);
            }
        }
    },

    addItem: function(id, text, value) {
        var item = this._addItemToCollection(id, text, value);
        this._addItem(item);
    },

    _addItemToCollection: function(id, text, value) {
        var fullID = this.get_element().id + "_" + id;
        var item = { "id": id, "text": text, "value": value != null ? value : id, "fullID": fullID };

        if (this._items == null) this._items = {};

        this._items[fullID] = item;

        this._count++;

        return item;
    },

    get_count: function() { return this._count; },

    clear: function() {
        this._count = 0;
        this._items = null;
        this.get_element().innerHTML = "";
    },

    _getSelection: function(dataOnly) {
        var data = [];
        var me = this;

        $(this.get_element()).find("input[type=checkbox]").each(
            function() {
                if (this.checked) {
                    if (dataOnly)
                        data[data.length] = (!me.get_booleanOptions() ? this.value : this.name);
                    else
                        data[data.length] = me._items[this.id];
                }
            }
        );

        if (data.length == 0)
            return null;
        else if (data.length == 1)
            return data[0] != this._allOptionID ? data[0] : null; //return 0 as null since it is considered a dummy id
        else
            return data;
    },

    get_values: function() {
        return this._getSelection(true);
    },

    get_selectedItems: function() {
        return this._getSelection(false);
    },

    get_text: function() {
        var data = [];
        var e = this.get_element();
        $(e).find("input[type=checkbox]").each(
            function() {
                if (this.checked) {
                    data[data.length] = $(e).find("label[for=" + this.id + "]").text();
                }
            }
        );

        if (data.length == 0)
            return this.get_defaultText();
        else if (data.length == 1)
            return data[0];
        else {
            //if multiitemtext was specified return that value for generic display when more than 1 item otherwise show list.
            if (this.get_multiItemText())
                return this.get_multiItemText();
            else
                return data.join(", ");
        }
    },

    getItem: function(id) {
        if (id.indexOf(this.get_element().id + "_") < 0)
            id = this.get_element().id + "_" + id;

        if (this._items)
            return this._items[id];

        return null;
    },

    getItemAt: function(index) {
        var id = null;
        var count = 0;
        if (this._items != null) {
            for (var item in this._items) {
                if (index == count) return this._items[item];
                count++;
            }
        }
        return null;
    },

    reset: function() {
        //if 0 element (default - all) then select otherwise clear all checks
        if (this._items == null || this.getItem(this.get_defaultValue()))
            this.selectItem(this.get_defaultValue());
        else {
            $(this.get_element()).find("input[type=checkbox]").each(function() { this.checked = false; });
        }
    },

    selectItem: function(id) {
        //must force checked = true and simulate click event since "click()" function in jQuery wasn't always working
        id = id.toString();
        if (id.indexOf(this.get_element().id + "_") < 0)
            id = this.get_element().id + "_" + id;

        var o = $("#" + id);
        if (o && o.length > 0) {
            o[0].checked = true;

            //fixes issue with losing "checked" value when hidden list is displayed
            if (ie6) //ie6 defined in UI.js
                o[0].defaultChecked = true;
            //fixes issue with losing "checked" value when hidden list is displayed

            //simulate click
            this._itemClicked({ target: o[0] });
        }
    },
    UnselectItem: function(id)
    {
        //must force checked = true and simulate click event since "click()" function in jQuery wasn't always working
        id = id.toString();
        if (id.indexOf(this.get_element().id + "_") < 0)
            id = this.get_element().id + "_" + id;

        var o = $("#" + id);
        if (o && o.length > 0)
        {
            o[0].checked =false;

            //fixes issue with losing "checked" value when hidden list is displayed
            if (ie6) //ie6 defined in UI.js
                o[0].defaultChecked = false;
            //fixes issue with losing "checked" value when hidden list is displayed

            //simulate click
            //this._itemClicked({ target: o[0] });
        }
    },
    selectItemAt: function(index) {
        var id = null;
        var count = 0;
        if (this._items != null) {
            for (var item in this._items) {
                id = item;
                if (index == count) break;
                count++;
            }
            if (index == count)
                this.selectItem(id);
        }
    },

    _addItem: function(item) {
        var e = this.get_element();

        var o = document.createElement("SPAN");
        o.innerHTML = this._getItemHTML(item, true);
        e.appendChild(o);

        if (this.get_breakCount() > 0 && ($("#" + e.id + " span").length % this.get_breakCount() == 0))
            e.appendChild(document.createElement("BR"));
    },

    _getItemCssClass: function(id) {
        var css = "";
        var itemCss = this.get_itemCssClass();

        if (id == 0 || itemCss) {
            css = "class='chkItem";
            if (id == 0) {
                css += " notfilter";
            }
            if (itemCss) {
                css += " " + itemCss;
            }
            css += "'";
        }
        return css;
    },

    _getItemHTML: function(item, innerOnly) {
        var id = item.fullID;
        var val = item.value;
        var name = $(this.get_element()).attr("name");

        if (this.get_booleanOptions()) {
            name = String.format(this.get_optionNameFormat(), val);
            val = this.get_trueValue();
        }

        var html = "<input type='checkbox' id='" + id + "' name='" + name + "' value='" + val + "' " + this._getItemCssClass(item.id) + " /><label for='" + id + "' " + (item.id == 0 ? "class='lableAll'" : "") + ">" + item.text + "</label>";

        return html;
    },

    _itemClicked: function(e) {
        if (e.stopPropagation) {
            e.stopPropagation();
        }
        var p = e.target;
        if (p.tagName == "LABEL") {
            return;
        }

        if (this._items == null) {
            var list = this;
            $("#" + this.get_element().id + " input").each(function() { list._addItemToCollection($simplifyName(this.id, "_"), "", this.value); });
        }

        var item = this._items[p.id];

        if (item) {
            if (item.id == this._allOptionID) {
                if (p.checked)
                    $("#" + this.get_element().id + " input").each(function() { if (this.id != p.id) { this.checked = false; /*this.disabled = p.checked;*/ } });
                else
                    p.checked = true;
            }
            else {
                var h;
                var selected = 0;

                $("#" + this.get_element().id + " input").each(function() { if (this.checked) selected++; });
                if (this.get_maxItems() > 0 && selected > this.get_maxItems()) {
                    //Keep last one if max is 1
                    if (this._maxItems == 1) {
                        $("#" + this.get_element().id + " input").each(function() { if (this.id != p.id) this.checked = false; });
                    }
                    //Else if max is > 1 then alert user
                    else {

                        p.checked = false;
                        h = this.get_events().getHandler("error");
                        if (h) h(this, new Sys.EventArgs());
                        return;
                    }
                }

                //If total checkboxes == total checkboxes not checked then default All (or defaultValue) option (if present) to Checked otherwise must be unchecked
                selected = 0;
                $("#" + this.get_element().id + " input").each(function() { if (this.checked) selected++; });
                var items = this._items;
                var defaultVal = this.get_defaultValue();
                if (defaultVal == this._allOptionID) {
                    $("#" + this.get_element().id + "_" + this.get_defaultValue()).each(function() { if (selected == 0) { this.checked = true; item = items[this.id]; } else if (defaultVal == 0) { this.checked = false; } });
                }
                else if (defaultVal == 0)
                {                    
                    $("#" + this.get_element().id + "_" + this._allOptionID).each(function() { if (selected == 0) { this.checked = true; item = items[this.id]; } else { this.checked = false; } });
                }
                else
                    $("#" + this.get_element().id + "_" + this.get_defaultValue()).each(function() { if (selected == 0) { this.checked = true; item = items[this.id]; } else if (defaultVal == 0) { this.checked = false; } });

            }
            h = this.get_events().getHandler("itemclicked");
            if (h) h(this, { 'item': item });
        }
    }

}
Pathfinder.UI.CheckboxList.registerClass('Pathfinder.UI.CheckboxList', Sys.UI.Control);



//----------------------------------------------------------Thin Grid
Pathfinder.UI.ThinGrid = function(element) {
    Pathfinder.UI.ThinGrid.initializeBase(this, [element]);

    this._onClickDelegate = null;
    this._onDataRequestedDelegate = null;

    this._url = null;
    this._params = {};

    this._loadSelector = null;

    this._multipleSelect = false;
    this._selection = [];

    this._autoLoad = false;
};

Pathfinder.UI.ThinGrid.prototype =
{
    initialize: function() {
        Pathfinder.UI.ThinGrid.callBaseMethod(this, 'initialize');

        this._onClickDelegate = Function.createDelegate(this, this._onClick);
        $addHandler(this.get_element(), "click", this._onClickDelegate);

        if (this.get_clientManager())
            this.get_clientManager().registerComponent(this.get_id(), null, this.get_autoUpdate(), this.get_containerID(), this.get_drillDownLevel());

        this._onDataRequestedDelegate = Function.createDelegate(this, this._onDataRequested);
        //this._onGridCountDelegate = Function.createDelegate(this, this._onGridCount);

        var autoLoad = this.get_autoLoad();
        var containerDiv = this.get_element();
        var staticHeader = this.get_staticHeader();
        var pagingEnabled = this.get_enablePaging();
        var getSelectedData = this.get_getSelectedData();

        if ((autoLoad && (!pagingEnabled)) || this.get_getSelectedData())
            this.dataBind();

        //        if ((autoLoad) && (containerDiv))
        //        {
        if (staticHeader)
            this.createStaticHeaders();
        //        }

        //new paging code
        //if (pagingEnabled)
        //    this.get_gridCount();
    },
    dispose: function() {
        if (this._onClickDelegate) {
            $removeHandler(this.get_element(), "click", this._onClickDelegate);
            delete this._onClickDelegate;
        }

        if (this._onDataRequested)
            delete this._onDataRequested;

        if (this._onGridCount)
            delete this._onGridCount;


        delete this._selection;

        Pathfinder.UI.ThinGrid.callBaseMethod(this, 'dispose');
    },

    add_dataBinding: function(handler) { this.get_events().addHandler("dataBinding", handler); },
    remove_dataBinding: function(handler) { this.get_events().removeHandler("dataBinding", handler); },

    add_dataBound: function(handler) { this.get_events().addHandler("dataBound", handler); },
    remove_dataBound: function(handler) { this.get_events().removeHandler("dataBound", handler); },

    add_rowSelecting: function(handler) { this.get_events().addHandler("rowSelecting", handler); },
    remove_rowSelecting: function(handler) { this.get_events().removeHandler("rowSelecting", handler); },

    add_rowSelected: function(handler) { this.get_events().addHandler("click", handler); },
    remove_rowSelected: function(handler) { this.get_events().removeHandler("click", handler); },

    add_click: function(handler) { this.add_rowSelected(handler); },
    remove_click: function(handler) { this.remove_rowSelected(handler); },

    get_url: function() { return this._url; },
    set_url: function(value) { this._url = value; },

    get_params: function() { if (!this.get_getSelectedData()) return this._params; else return clientManager.cleanSelectionData(clientManager.get_SelectionData()); },
    set_params: function(value) {
        if (!value)
            this._params = {};
        else {
            if (typeof value == "string")
                value = Sys.Serialization.JavaScriptSerializer.deserialize(value, true);
            this._params = value;
        }
    },

    get_allowMultiSelect: function() { return this._multipleSelect; },
    set_allowMultiSelect: function(value) { this._multipleSelect = value; },

    get_autoLoad: function() { return this._autoLoad; },
    set_autoLoad: function(value) { this._autoLoad = value; },

    get_autoUpdate: function() { return this._autoUpdate; },
    set_autoUpdate: function(value) { this._autoUpdate = value; },

    get_drillDownLevel: function() { return this._drillDownLevel; },
    set_drillDownLevel: function(value) { this._drillDownLevel = value; },

    get_loadSelector: function() { return this._loadSelector; },
    set_loadSelector: function(value) { this._loadSelector = value; },

    get_containerID: function() { return this._containerID; },
    set_containerID: function(value) { this._containerID = value; },

    get_staticHeader: function() { return this._staticHeader; },
    set_staticHeader: function(value) { this._staticHeader = value; },

    get_enablePaging: function() { return this._enablePaging; },
    set_enablePaging: function(value) { this._enablePaging = value; },

    get_pageSize: function() { return this._pageSize; },
    set_pageSize: function(value) { this._pageSize = value; },

    get_pageSelector: function() { return this._pageSelector; },
    set_pageSelector: function(value) { this._pageSelector = value; },

    get_pageContainer: function() { return this._pageContainer; },
    set_pageContainer: function(value) { this._pageContainer = value; },

    get_pageNumber: function() { return this._pageNumber; },
    set_pageNumber: function(value) { this._pageNumber = value; },
    //
    get_requestPageCount: function() { return this._requestPageCount; },
    set_requestPageCount: function(value) { this._requestPageCount = value; },

    get_getSelectedData: function() { return this._getSelectedData; },
    set_getSelectedData: function(value) { this._getSelectedData = value; },
    //
    get_clientManager: function() { return window.top.clientManager; },

    get_updating: function() { return this._updating; },

    dataBind: function() {
        var h = this.get_events().getHandler("dataBinding");
        if (h) {
            var e = new Sys.CancelEventArgs();
            h(this, e);
            if (e.get_cancel())
                return;
        }

        var loadSelector = this.get_loadSelector();
        if (!loadSelector) loadSelector = "form>*";

        this.clearSelections();

        if (this.get_url()) {
            var q = this.get_params();

            var pagingEnabled = this.get_enablePaging();

            if (pagingEnabled) {
                //Get full grid count
                if (this.get_requestPageCount()) {
                    this.clearSelections();

                    q["RequestPageCount"] = true;
                }
                else
                    q["RequestPageCount"] = false;

                q["PagingEnabled"] = this.get_enablePaging();
                q["TotalPerPage"] = this.get_pageSize();

                var pageNumber = this.get_pageNumber();
                if (pageNumber)
                    q["StartPage"] = pageNumber;
                else {
                    q["StartPage"] = 1;
                    this.set_pageNumber(1);
                }
            }


            this._updating = true;

            $(this.get_element()).load(this.get_url() + " " + loadSelector, $.param(q, true), this._onDataRequestedDelegate);
        }

    },

    //    get_gridCount: function()
    //    {
    //        this.clearSelections();

    //        var pageSelector = this.get_pageSelector();
    //        var pageContainer = this.get_pageContainer();

    //        var q = this.get_params();

    //        q["PagingEnabled"] = false;

    //        if (this.get_url())
    //            $(pageContainer).load(this.get_url() + " " + pageSelector, $.param(q, true), this._onGridCountDelegate);
    //    },

    get_values: function() { return this.selectedValues(); },

    constructPager: function(grid, count) {
        var gridID = grid.id;
        var pageHTML = "";
        var size = this.get_pageSize();

        var page = this.get_pageNumber() - 1; //(!customPaging ? mt.get_currentPageIndex() : getPageIndexFromFilter(mt));

        var buttonCount = 5;
        var pageCount = Math.ceil(count / size);
        var first = (page * size) + 1;
        var last = (first - 1 + size);
        var basePath = this.get_clientManager().get_BasePath();
        var customPaging;

        if (last > count) last = count;

        //Pager Text
        //var pagerText = (mt.get_owner().get_element().control.GridWrapper && mt.get_owner().get_element().control.GridWrapper.get_showNumberOfRecords() ? "<span class='pagerText'>" + String.format("Records {0}-{1} of {2}", first, last, count) + "</span>" : "");
        var pagerText = "";

        //Pager Page Buttons
        var pagerButtons = "";
        var buttonStart = parseInt(page / buttonCount) * buttonCount;
        var buttonEnd = buttonStart + buttonCount;
        if (buttonEnd > pageCount) buttonEnd = pageCount;

        for (var i = buttonStart; i < buttonEnd; i++) {
            if (page != i)
                pagerButtons += "<a class='pg" + i + "' href='javascript:void(0)' onclick='" + "setGridPage(\"" + gridID + "\"," + i + "," + count + ")'>" + (i + 1) + "</a>";
            else
                pagerButtons += "<span>" + (i + 1) + "</span>";
        }

        //Pager Next/Previous Buttons
        var pagerPrevious;
        if (buttonStart > 0)
            pagerPrevious = "<img class='pagerPrev' src='" + basePath + "/content/images/spacer.gif' onclick='" + "setGridPage(\"" + gridID + "\"," + (buttonStart - 1) + "," + count + ")' />";
        else
            pagerPrevious = "<img class='pagerPrev grey' src='" + basePath + "/content/images/spacer.gif' />";

        var pagerNext;
        if (buttonEnd < pageCount)
            pagerNext = "<img class='pagerNext' src='" + basePath + "/content/images/spacer.gif' onclick='" + "setGridPage(\"" + gridID + "\"," + (buttonStart + buttonCount) + "," + count + ")' />";
        else
            pagerNext = "<img class='pagerNext grey' src='" + basePath + "/content/images/spacer.gif' />";

        //Output pager HTML
        pagerHTML = "<div>" + pagerPrevious + "<div class='pagerButtons' style='display:inline'>" + pagerButtons + "</div>" + pagerNext + pagerText + "</div>";

        return pagerHTML;
    },

    setGridPage: function(grid, page, totalCount) {

        //        var g = $get(grid);
        //        if (g && g.control)
        //        {
        //            g = g.control;

        //            if (g.get_masterTableView)
        //                g = g.get_masterTableView();

        //            if (!customPaging)
        //                g.set_currentPageIndex(page);
        //            else
        //            {
        //                $setGridFilter(g, "Page_Index", page, "EqualTo", "System.Int32");
        //                g.rebind();
        //            }

        //            //update pager text with new page before data is even returned for better user feel - if counts actually change the pager will be updated again once data is loaded
        //            var w = g.get_parent().get_element();
        //            w = w.control.GridWrapper;
        //            if (w)
        //            {
        //                $(w.get_pagerSelector()).html($constructCustomPager(g, g.get_virtualItemCount(), false, w.get_clientManager().get_BasePath(), customPaging));
        //                if (w.get_showLoading())
        //                    $(w.get_pagerSelector()).find(" .pagerText").html("<span class='loading'>" + w.get_loadingText() + "</span>");
        //            }
        //        }
    },

    selectedValues: function(selector) {
        var vals = [];
        var func = "val";
        if (!selector) {
            selector = "#dataKey";
        }
        else
            func = "text";

        for (var i = 0; i < this._selection.length; i++) {
            vals[i] = $(this._selection[i]).find(selector)[func]();
        }

        return vals;
    },

    selectRowByDatakey: function(key) {
        if (key) {
            //handle selections
            if (!this._multipleSelect) //if not multi select reinit selections
                this.clearSelections();

            var containerDiv = this.get_element();
            var tbl = $(containerDiv).find("table");

            var row = $(tbl).find("input:hidden[value='" + key + "']").parents("tr:first");

            if (row)
                this.selectRow(row[0]);
        }
    },

    selectRow: function(row) {
        if (row) {
            var initL = this._selection.length;
            this._selection = $.grep(this._selection, function(i, x) { return i != row; });
            var add = initL == this._selection.length;
            if (add) {
                this._selection[this._selection.length] = row;
                $(row).addClass("selected");
            }
            else
                $(row).removeClass("selected");
        }
    },

    clearSelections: function() {
        for (var i = 0; i < this._selection.length; i++)
            $(this._selection[i]).removeClass("selected");

        this._selection = [];
    },

    createStaticHeaders: function() {
        var containerDiv = this.get_element();
        var containerDivID = $(containerDiv).attr("id");
        var tbl = $(containerDiv).find("table");
        var tblID = containerDivID;  //$(tbl).attr("id");
        var clonedRow = $("#" + tblID + " tr:first").clone();

        if (clonedRow.length > 0) {
            $("#" + tblID + "_gridCloneDiv").remove();

            $(containerDiv).before("<div class='cloneHeaderFull'><div id='" + tblID + "_gridCloneDiv' style='margin-right: 17px; overflow: hidden;' class='cloneHeader'><table  class='gridClone' id='" + tblID + "_gridClone' >" + clonedRow[0].innerHTML + "</table></div></div>");

            $(containerDiv).scroll(function() {
                $("#" + tblID + "_gridCloneDiv").scrollLeft($(this).scrollLeft());
            });

            var parent = $(containerDiv).parent();

            var j = $(containerDiv);
            j.height(safeSub(j.parent().height(), (Sys.UI.DomElement.getBounds(j[0]).y - Sys.UI.DomElement.getBounds(j.parent()[0]).y)));

            //$(tbl).css("margin-top", "-" + $("#" + tblID + "_gridCloneDiv").height() + "px");
            $(tbl).css("margin-top", "-" + ($("#" + tblID + " tr:first").css("visibility", "hidden").height()) + "px");

            $("#" + tblID + "_gridClone").width($(tbl).width());

            setTimeout("resizeStaticHeaders('" + containerDivID + "')", 1);
        }
    },

    _onDataRequested: function(html, status) {
        var d = document.createElement("DIV");
        d.innerHTML = html;

        //responseStatus is supposed to equal "timeout" if response header __pinsologin is true (set in loadEx function) but custom hdrs are only supported in IIS 7.0 integrated pipeline mode which requires different dev environment
        if ($(d).find("#loginPage").length == 0) {
            var containerDiv = this.get_element();
            var staticHeaders = this.get_staticHeader();
            //Static Header code
            if (staticHeaders)
                this.createStaticHeaders();

            //
            if (this.get_requestPageCount()) {
                var pageSelector = this.get_pageSelector();
                var pageContainer = this.get_pageContainer();
                var rowCount = $(pageSelector).text();

                $(pageContainer).html(this.constructPager(this.get_element(), rowCount));

                //No longer request page count 
                this.set_requestPageCount(false);
            }
            //

            if (this.get_clientManager())
                this.get_clientManager().get_ApplicationManager().resizeSection();
        }
        else //timeout occurred but hdr not set (see above comment)
        {
            $(this.get_element()).html("");

            //window.top.location = "login.aspx";
            if (this.get_clientManager())
                this.get_clientManager().validateCurrentUser();
        }
        this._updating = false;

        h = this.get_events().getHandler("dataBound");
        if (h)
            h(this, new Sys.EventArgs());
    },

    //    _onGridCount: function(results, status)
    //    {
    //        //var containerDiv = this.get_element();
    //        //var pageSelector = this.get_pageSelector();
    //        var pageContainer = this.get_pageContainer();
    //        var rowCount = $(pageContainer).text();

    //        $(pageContainer).html(this.constructPager(this.get_element(), rowCount));
    //    },

    _onClick: function(e) {
        var target = e.target;
        if (target.tagName != "TR") {
            while (target && target.tagName != "TR")
                target = target.parentNode;
        }

        if (target) {
            var h = this.get_events().getHandler("rowSelecting");
            if (h) {
                var e = new Sys.CancelEventArgs();
                if (h) h(this, e);
                if (e.get_cancel())
                    return;
            }

            //handle selections
            if (!this._multipleSelect) //if not multi select reinit selections
                this.clearSelections();

            this.selectRow(target);

            var id = $(target).find("#dataKey").val();

            h = this.get_events().getHandler("click");
            if (h) h(this, { ID: id });
        }
    }
};
Pathfinder.UI.ThinGrid.registerClass('Pathfinder.UI.ThinGrid', Sys.UI.Control);

//----------------------------------------------------------Menu/Toolbar

Pathfinder.UI.Menu = function(element) {
    Pathfinder.UI.Menu.initializeBase(this, [element]);

    this._items = {};
    this._overflowElement = null;

    this._cssClass = "coreBtn";
    this._selectedCssClass = "selected";

    this._selectedItem = null;

    this._menuClickedDelegate = null;

    this._renderStyle = 1; //1 = div wrapper with span buttons inside <span class=\"bg\"><span class=\"bg2\"></span></span>.  2 = ul with list items containing <span><a></a></span>
};

Pathfinder.UI.Menu.prototype =
{
    initialize: function() {
        Pathfinder.UI.Menu.callBaseMethod(this, 'initialize');

        var e = this.get_element();

        if (this.get_renderStyle() == 1) {
            e.innerHTML = "<div style='display:inline'></div>";
            //            e.style.overflow = "hidden";
        }
        else {
            e.innerHTML = "<ul class='ui-tabs-nav'></ul>";
        }

        //        this._overflowElement = document.createElement("DIV");
        //        this._overflowElement.style.display = "none";
        //        this._overflowElement.position = "absolute";
        //        e.appendChild(this._overflowElement);

        //        $addHandler(window, "resize", (this._windowResizeDelegate = Function.createDelegate(this, this._windowResize)));
        $addHandler(e, "click", (this._menuClickedDelegate = Function.createDelegate(this, this._menuClicked)));

        var h = this.get_events().getHandler("initialized");
        if (h) h(this, new Sys.EventArgs());
    },
    dispose: function() {
        if (this._menuClickedDelegate) {
            try { $removeHandler(this.get_element(), "click", this._menuClickedDelegate); } catch (ex) { }
            delete (this._menuClickedDelegate);
        }

        //        if (this._windowResizeDelegate)
        //            $removeHandler(window, "resize", this._windowResizeDelegate);
        //       delete (this._windowResizeDelegate);

        Pathfinder.UI.Menu.callBaseMethod(this, 'dispose');
    },

    add_initialized: function(handler) { this.get_events().addHandler("initialized", handler); },
    remove_initialized: function(handler) { this.get_events().removeHandler("initialized", handler); },

    get_cssClass: function() { return this._cssClass; },
    set_cssClass: function(value) { this._cssClass = value; },

    get_selectedCssClass: function() { return this._selectedCssClass; },
    set_selectedCssClass: function(value) { this._selectedCssClass = value; },

    add_itemClicked: function(handler) { this.get_events().addHandler("itemclicked", handler); },
    remove_itemClicked: function(handler) { this.get_events().removeHandler("itemclicked", handler); },

    get_selectedItem: function() { return this._selectedItem; },

    get_renderStyle: function() { return this._renderStyle; },
    set_renderStyle: function(value) {
        if (value != this._renderStyle) {
            if (value == 1)
                this.get_element().innerHTML = "<div style='display:inline'></div>";
            else
                this.get_element().innerHTML = "<ul class='ui-tabs-nav'></ul>";

            this.clear();
        }
        this._renderStyle = value;
    },

    addItem: function(id, text, value, altCss) {
        var item = { "id": id, "text": text, "value": value != null ? value : id };
        this._items[id] = item;
        this._addItem(item, altCss);
    },

    clear: function() {
        this._selectedItem = null;
        this._items = {};
        if (this.get_element().childNodes.length > 0)
            this.get_element().childNodes[0].innerHTML = "";
    },

    getItem: function(id) {
        return this._items[id];
    },

    getItemAt: function(index) {
        var id = null;
        var count = 0;
        for (var item in this._items) {
            if (index == count) return this._items[item];
            count++;
        }

        return null;
    },

    selectItem: function(id) {
        this._selectedItem = null;
        $("#" + id).click();
    },

    selectItemAt: function(index) {
        var id = null;
        var count = 0;
        for (var item in this._items) {
            id = item.id;
            if (index == count) break;
            count++;
        }
        if (index == count)
            this.selectItem(id);
    },

    highlightItem: function(id) {
        if (id == null || id == "") return;

        if (this.get_renderStyle() == 1) {
            $("#" + this.get_element().id + " a").removeClass(this.get_selectedCssClass());
            $("#" + this.get_element().id + " ." + this.get_cssClass() + " span").parent("span").removeClass(this.get_selectedCssClass());
        }
        else
            $("#" + this.get_element().id + " li").removeClass(this.get_selectedCssClass());

        $("#" + id).addClass(this.get_selectedCssClass());

        if (this.get_renderStyle() == 1)
            $("." + this.get_selectedCssClass()).parent("span").parent("span").parent("span").addClass(this.get_selectedCssClass());
    },

    highlightItemAt: function(index) {
        var id = null;
        var count = 0;
        for (var item in this._items) {
            id = this._items[item].id;
            if (index == count) break;
            count++;
        }
        if (index == count)
            this.highlightItem(id);
    },

    _addItem: function(item, altCss) {
        var e = this.get_element().childNodes[0];

        var tagName = this.get_renderStyle() == 1 ? "SPAN" : "LI";

        var o = document.createElement(tagName);
        o.className = this.get_cssClass() + (altCss ? " " + altCss : "");
        o.id = item.id;
        o.innerHTML = this._getItemHTML(item, true);

        e.appendChild(o);
    },

    addDivider: function() {
        var e = this.get_element().childNodes[0];

        var tagName = this.get_renderStyle() == 1 ? "SPAN" : "LI";

        var o = document.createElement(tagName);
        o.innerHTML = "<span><hr></hr></span>";

        e.appendChild(o);
    },

    _getItemHTML: function(item, innerOnly) {
        var html = "";

        //        if (!innerOnly)
        //            html += "<span class='coreBtn' id='" + item.id + "'>";
        if (this.get_renderStyle() == 1)
            html += "<span class=\"bg\"><span class=\"bg2\"><a href=\"javascript:void(0);\" class=\"button\">" + item.text + "</a></span></span>";
        else
            html += "<span><a href=\"javascript:void(0);\">" + item.text + "</a></span>";
        //        if (!innerOnly)
        //            html += "</span>";

        return html;
    },

    _menuClicked: function(e) {
        var p = e.target;
        if (p.tagName == "A") {
            var p = p.parentNode;
            while (p != null && p.id == "")
                p = p.parentNode;
        }

        var item = this._items[p.id];

        if (item != null) {
            this.highlightItem(item.id);

            this._selectedItem = item;

            var h = this.get_events().getHandler("itemclicked");
            if (h) h(this, { 'item': item });
        }
    },

    _windowResize: function() {
        //reset all

        //check overflow   
    }
};
Pathfinder.UI.Menu.registerClass('Pathfinder.UI.Menu', Sys.UI.Control);



//----------------------------------------------------SEARCH LIST CONTROL------------------------------------------------------
//var searchListMultiSelectOptions = [];
var searchListControl = null;

Pathfinder.UI.SearchList = function(element) {
    Pathfinder.UI.SearchList.initializeBase(this, [element]);

    this.serviceUrl = null;
    this.queryFormat = null;
    this.queryValues = [];
    this.dataField = null;
    this.textField = null;
    this.multiSelectHeaderText = null;
    this.waterMarkText = null;
    this.searchListMultiSelectOptions = [];
    this.hiddenCtrlID = null;
    this.multiSelect = null;
    this.ctrlID = null;
    this._maxHeight = 300;



    this._offsetX = 0;
    this._offsetY = 0;

    this._clientManager = null;
    this._containerID = null;

    this._searchCallbackDelegate = null;
    this._queueSearchRequestDelegate = null;
    this._hideSearchListDelegate = null;
    this._hideSearchListSelectionDelegate = null;
    this._itemClickedDelegate = null;
};

Pathfinder.UI.SearchList.prototype =
{
    initialize: function()
    {
        Pathfinder.UI.SearchList.callBaseMethod(this, 'initialize');

        var e = this.get_element();

        this.ctrlID = e.id;

        if (this.waterMarkText)
            $('#' + this.ctrlID).watermark(this.waterMarkText);

        var o = $get("searchList");
        //if (!o) {
        o = document.createElement("DIV");
        if (ie7)
            $(o).css({ "z-index": "3000", display: "none", "text-align": "left", position: "absolute", "background-color": "#fff", border: "solid 2px #2d58a7", height: 300, overflow: "scroll", "overflow-x": "visible" }).attr("id", "searchList" + e.id);
        else
            $(o).css({ "z-index": "3000", display: "none", "text-align": "left", position: "absolute", "background-color": "#fff", border: "solid 2px #2d58a7", height: 300, overflow: "auto", "overflow-x": "visible" }).attr("id", "searchList" + e.id);

        document.body.appendChild(o);

        if (this.multiSelect == true)
        {
            oo = document.createElement("DIV");
            if (ie7)
                $(oo).css({ "z-index": "2999", display: "none", "text-align": "left", position: "absolute", "background-color": "#fff", border: "solid 2px #2d58a7", height: 300, overflow: "scroll", "overflow-x": "visible" }).attr("id", "searchListSelection" + e.id);
            else
                $(oo).css({ "z-index": "2999", display: "none", "text-align": "left", position: "absolute", "background-color": "#fff", border: "solid 2px #2d58a7", height: 300, overflow: "auto", "overflow-x": "visible" }).attr("id", "searchListSelection" + e.id);

            var ooHeader = document.createElement("DIV");
            var ooClearDiv = document.createElement("DIV");
            var ooWidthDiv = document.createElement("DIV");
            if (ie7)
                $(ooHeader).css({ "position": "relative", "background-color": "#2d58a7", "width": "auto", "top": "0px", "left": "0px", "padding-bottom": "4px", "padding-top": "4px", "color": "white" }).addClass('title1').attr("id", "searchListSelectionHeader" + e.id).html('&nbsp;' + this.multiSelectHeaderText);
            else
                $(ooHeader).css({ "position": "relative", "background-color": "#2d58a7", "width": "100%", "top": "0px", "left": "0px", "padding-bottom": "4px", "padding-top": "4px", "color": "white" }).addClass('title1').attr("id", "searchListSelectionHeader" + e.id).html('&nbsp;' + this.multiSelectHeaderText);
            $(ooClearDiv).addClass('clearAll');
            $(ooWidthDiv).css({ "width": "150px", "height": "1px" }).attr("id", "searchListSelectionHeaderWidth" + e.id);

            $addHandler(oo, "click", this.onSelectionRemoveClick);
            $addHandler(o, "click", this.onSelectionAddClick);

            oo.appendChild(ooWidthDiv);
            oo.appendChild(ooClearDiv);
            oo.appendChild(ooHeader);
            oo.appendChild(ooClearDiv);

            document.body.appendChild(oo);
        }


        //            if (chrome)
        //                $(o).css("overflow", "auto").css("overflow-x", "visible");
        //}

        if (this.dataField != this.textField)
        {
            var id = this.get_element().id + "_DATA";
            this.hiddenCtrlID = id;
            o = $get(id);
            if (!o)
            {
                o = document.createElement("INPUT");
                $(o).attr("type", "hidden").attr("id", id).attr("name", this.get_element().id);
                $(this.get_element()).parent().append(o);
            }
        }

        this._queueSearchRequestDelegate = Function.createDelegate(this, this._queueSearchRequest);
        $addHandler(e, "click", (this._itemClickedDelegate = Function.createDelegate(this, this._itemClicked)));

        if (this.get_clientManager())
            this.get_clientManager().registerComponent(this.get_id(), null, null, this.get_containerID());

        $(this.get_element()).keyup(this._queueSearchRequestDelegate).keydown(this._queueSearchRequestDelegate);

        searchListControl = this;
    },

    dispose: function()
    {
        this.hideSearchList(true);
        this.hideSearchListSelection(true);

        Pathfinder.UI.SearchList.callBaseMethod(this, 'dispose');

        $(this.get_element()).unbind("keydown", this._queueSearchRequestDelegate);
        $(this.get_element()).unbind("keyup", this._queueSearchRequestDelegate);

        if (this._itemClickedDelegate)
        {
            try { $removeHandler(this.get_element(), "click", this._itemClickedDelegate); } catch (ex) { }
            delete (this._itemClickedDelegate);
        }

        if (this.multiSelect == true)
        {
            //$removeHandler($get("searchListSelection"), "click", this.onSelectionRemoveClick);
            //$removeHandler($get("searchList"), "click", this.onSelectionAddClick);
            $('#searchListSelection div:not(#searchListSelection div.title1, #searchListSelection #searchListSelectionHeaderWidth, #searchListSelection .clearAll)').remove();
        }

        delete this._searchCallbackDelegate;
        delete this._queueSearchRequestDelegate;
        delete this._hideSearchListDelegate;
        delete this._hideSearchListSelectionDelegate;
    },

    get_serviceUrl: function() { return this.serviceUrl; },
    set_serviceUrl: function(value) { this.serviceUrl = value; },

    get_queryFormat: function() { return this.queryFormat; },
    set_queryFormat: function(value) { this.queryFormat = value; },

    get_queryValues: function() { return this.queryValues; },
    set_queryValues: function(value)
    {
        if (typeof value == "string")
            value = eval(value);

        if ($.isArray(value))
            this.queryValues = value;
        else if (value != null)
            this.queryValues = [value];
        else
            this.queryValues = [];
    },

    get_dataField: function() { return this.dataField; },
    set_dataField: function(value) { this.dataField = value; },

    get_textField: function() { return this.textField; },
    set_textField: function(value) { this.textField = value; },

    get_dataValue: function() { return this.dataValue; },
    set_dataValue: function(value) { this.dataValue = value; },

    get_containerID: function() { return this._containerID; },
    set_containerID: function(value) { this._containerID = value; },

    get_clientManager: function() { return this._clientManager; },
    set_clientManager: function(value) { this._clientManager = value; },

    get_multiSelect: function() { return this.multiSelect; },
    set_multiSelect: function(value) { this.multiSelect = value; },

    get_multiSelectHeaderText: function() { return this.multiSelectHeaderText; },
    set_multiSelectHeaderText: function(value) { this.multiSelectHeaderText = value; },

    get_waterMarkText: function() { return this.waterMarkText; },
    set_waterMarkText: function(value) { this.waterMarkText = value; },

    get_maxHeight: function() { return this._maxHeight; },
    set_maxHeight: function(value) { this._maxHeight = value; },

    get_offsetX: function() { return this._offsetX; },
    set_offsetX: function(value) { this._offsetX = value; },

    get_offsetY: function() { return this._offsetY; },
    set_offsetY: function(value) { this._offsetY = value; },

    get_multipleSelectedValues: function() { return this._multipleSelectedValues; },
    set_multipleSelectedValues: function(value) { this._multipleSelectedValues = value; },

    _queueSearchRequest: function(e)
    {

        if (e.type == "keyup")
        {
            //Don't execute for Arrow keys or other non-character buttons
            if ((e.keyCode >= 37 && e.keyCode <= 40) //arrows
            || (e.keyCode >= 16 && e.keyCode <= 18) //CTRL, Shift, Alt
            || (e.keyCode >= 33 && e.keyCode <= 36) //Home, End, P-Up, P-Down
            || e.keyCode == 20
            || e.keyCode == 91
            || e.keyCode == 93) return;

            if (this._searchCmd)
            {
                this._searchCmd.cancel();
                delete this._searchCmd;
            }

            var args = Array.clone(this.queryValues);
            args[args.length] = encodeURIComponent($.trim(e.target.value)).replace(/'/ig, "%27%27");

            this._searchCmd = new cmd(this, "search", [e.target, args], 500);
        }
        else if (e.type == "keydown")
        {
            if (e.keyCode == 9) //close if tabbing out (handling blur causes issues with clicking on scrollbar since it triggers a blur event and then closes list)
                this.hideSearchList();
            else if (e.keyCode == 40 && $("#searchList" + this.ctrlID).html()) //down arrow and contents present
                this.showSearchList();
            else if (e.keyCode == 38)
                this.hideSearchList();
        }
    },

    clear: function()
    {
        $("#searchList" + this.ctrlID).html("");
        this.hideSearchList(true);
    },

    clearSearchListSelection: function()
    {
        $('#searchListSelection div:not(#searchListSelection div.title1, #searchListSelection #searchListSelectionHeaderWidth, #searchListSelection .clearAll)').remove();
        $('#' + searchListControl.hiddenCtrlID).val('');
        //to clear the previous selected values
        searchListControl.searchListMultiSelectOptions = [];
    },

    search: function(sender, args)
    {
        if (!this._searchCallbackDelegate)
            this._searchCallbackDelegate = Function.createDelegate(this, this.searchCallback);

        Array.insert(args, 0, this.queryFormat);
        var url = this.serviceUrl + "?" + String["format"].apply(String, args);

        //        if (this.get_clientManager()) this.get_clientManager().ajaxRequestStarting();

        $.getJSON(url, null, this._searchCallbackDelegate);
    },

    searchCallback: function(result, status)
    {
        //        if (this.get_clientManager()) this.get_clientManager().ajaxRequestComplete();
        //var id = "#searchList" + this.ctrlID;
        var d = result.d;
        if (d && d.length > 0)
        {
            var rect = Sys.UI.DomElement.getBounds(this.get_element());

            //set width of list to same as textbox - it will be cleared and reset if needed after the contents load.
            $("#searchList" + this.ctrlID).css("width", rect.width - this.get_offsetX());

            var s = "";

            if (this.multiSelect == false)
            {
                for (var i = 0; i < d.length; i++)
                {
                    s += "<div style='margin:1px 2px 1px 2px;height:18px;white-space:nowrap'><a class='button' href='javascript:void(0)' onclick='$(\"#" + this.get_element().id + "\").val(decodeURIComponent(\"" + d[i][this.textField].replace(/'/ig, "%27") + "\")).focus();$(\"#" + this.get_element().id + "_DATA\").val(decodeURIComponent(\"" + d[i][this.dataField].toString().replace(/'/ig, "%27") + "\"));'>" + d[i][this.textField] + "</a></div>";
                }
            }
            else
            {
                for (var i = 0; i < d.length; i++)
                {
                    //only add element is it if not already selected
                    var found = $('#searchListSelection' + this.ctrlID + ' :contains("' + d[i][this.textField] + '")').length;
                    if (found == 0)
                        s += "<div style='margin:1px 2px 1px 2px;height:18px;white-space:nowrap'><a class='button' href='javascript:void(0)' id='" + d[i][this.dataField].toString() + "'>" + d[i][this.textField] + "<img src='App_Themes/pathfinder/images/close.gif' /></a></div>";
                }
            }

            var h = d.length * 20;
            //check max height to see if list is over
            if (h > this.get_maxHeight()) h = this.get_maxHeight();
            //check if list goes past window bottom - not flipping above list - just resizing to lesser amount.
            var top = rect.y + rect.height + 2 + this.get_offsetY();
            if (h + top > $(window).height())
                h = safeSub(h, ((h + top) - $(window).height()) + 5);

            var searchListVisible = $("#searchList" + this.ctrlID).is(":visible");

            $("#searchList" + this.ctrlID).css("left", rect.x + 2 + this.get_offsetX()).css("top", top).height(h).html(s).slideDown(250, function()
            {
                if (chrome)
                    $("#searchList" + this.ctrlID).css("overflow-x", "visible").css("overflow-y", "auto");

                var w = (rect.width - 4);
                if (!ie6)
                {
                    $("#searchList" + this.ctrlID).css("width", "");
                }
                if ($("#searchList" + this.ctrlID).width() < w)
                    $("#searchList" + this.ctrlID).width(w);
                else if (chrome)
                    $("#searchList" + this.ctrlID).width($("#searchList" + this.ctrlID).width() + 20);
            }
            );

            if (this.multiSelect == true)
            {
                var aa = $("#searchList" + this.ctrlID).width();
                var ht = $("#searchListSelection" + this.ctrlID).html();

                if (h < 25)
                    h = 100;
                var ID = this.ctrlID;
                $("#searchListSelection" + this.ctrlID).css("top", top).height(h).html(ht).slideDown(250, function()
                {
                    //Only animate if searchList is not visible
                    if (ie7)
                    {
                        if (searchListVisible == false)
                            $("#searchListSelection" + ID).delay(400).animate({ 'left': $("#searchList" + ID).width() + 23 }, 250, 'linear', null);
                        //setTimeout("$('#searchListSelection" + this.ctrlID + "').animate({ 'left': $('#searchList" + this.ctrlID + "').width() + 23 }, 250, 'linear', null);", 400);
                        else
                            $("#searchListSelection" + ID).delay(25).css('left', $("#searchList" + ID).width() + 23);
                        //setTimeout("$('#searchListSelection" + this.ctrlID + "').css('left', $('#searchList" + this.ctrlID + "').width() + 23)", 25);
                    }
                    else
                    {
                        if (searchListVisible == false)
                            $("#searchListSelection" + ID).delay(400).animate({ 'left': $("#searchList" + ID).width() + 18 });
                        //setTimeout("$('#searchListSelection" + this.ctrlID + "').animate({ 'left': $('#searchList" + this.ctrlID + "').width() + 18 }, 250, 'linear', null);", 400);
                        else
                            $("#searchListSelection" + ID).delay(25).css('left', $("#searchList" + ID).width() + 18);
                        //setTimeout("$('#searchListSelection" + this.ctrlID + "').css('left', $('#searchList" + this.ctrlID + "').width() + 18)", 25);
                    }
                }
            );

            }

            //Remove possible duplicates
            var seen = {};
            $('#searchList' + this.ctrlID + ' div').each(function()
            {
                var txt = $(this).text();
                if (seen[txt])
                    $(this).remove();
                else
                    seen[txt] = true;
            });

            if (!this._hideSearchListDelegate)
            {
                this._hideSearchListDelegate = Function.createDelegate(this, this.hideSearchList);
                $(document).click(this._hideSearchListDelegate);
            }

            if (this.multiSelect == true && (!this._hideSearchListSelectionDelegate))
            {
                this._hideSearchListSelectionDelegate = Function.createDelegate(this, this.hideSearchListSelection);
                $(document).click(this._hideSearchListSelectionDelegate);
            }
        }
        else
        {
            $("#searchList" + this.ctrlID).html("");
            $("#" + this.get_element().id + "_DATA").val("");
            this.hideSearchList(true);

            if (this.multiSelect == true)
                this.hideSearchListSelection(true);
        }
    },

    showSearchList: function()
    {
        if (!this._hideSearchListDelegate)
        {
            this._hideSearchListDelegate = Function.createDelegate(this, this.hideSearchList);
            $(document).click(this._hideSearchListDelegate);
        }

        $("#searchList" + this.ctrlID).slideDown(250);
    },

    hideSearchList: function(e)
    {
        //Check if click came from inside searchlist and if multiselect is enabled
        //We don't want to close searchlist if click came from inside searchlist and multiselect is enabled
        if (this.multiSelect == true && e != true && ($(e.srcElement).parents("#searchList" + this.ctrlID).length > 0 || $(e.srcElement).parents("#searchListSelection" + this.ctrlID).length > 0 || e.srcElement.id == "searchListSelection" + this.ctrlID || e.srcElement.nameProp == "close.gif" || e.srcElement.id == "searchList" + this.ctrlID))
            return;
        else
        {
            if (e == true)
                $("#searchList" + this.ctrlID).hide();
            else
            {
                if (!e || e.target.id != this.get_element().id)
                    $("#searchList" + this.ctrlID).slideUp(250);
                else
                    return; //clicked in target control
            }

            if (this._hideSearchListDelegate)
            {
                $(document).unbind("click", this._hideSearchListDelegate);

                delete this._hideSearchListDelegate;
            }
        }
    },

    hideSearchListSelection: function(e)
    {
        //Check if click came from inside searchlist and if multiselect is enabled
        //We don't want to close searchlist if click came from inside searchlist and multiselect is enabled
        if (this.multiSelect == true && e != true && ($(e.srcElement).parents("#searchList" + this.ctrlID).length > 0 || $(e.srcElement).parents("#searchListSelection" + this.ctrlID).length > 0 || e.srcElement.id == "searchListSelection" + this.ctrlID || e.srcElement.id == "searchList" + this.ctrlID))
        {
            //Remove hidden divs from searchListSelection
            $("#searchListSelection" + this.ctrlID + " div:hidden").remove();
            return;
        }
        else
        {
            if (e == true)
            {
                $("#searchListSelection" + this.ctrlID).hide();
                $("#" + this.ctrlID).val('');
            }
            else
            {
                if (!e || e.target.id != this.get_element().id)
                {
                    $("#searchListSelection" + this.ctrlID).slideUp(250);
                    $("#" + this.ctrlID).val('');
                }
                else
                    return; //clicked in target control
            }

            if (this._hideSearchListSelectionDelegate)
            {
                $(document).unbind("click", this._hideSearchListSelectionDelegate);

                delete this._hideSearchListSelectionDelegate;
            }
        }
    },
    _itemClicked: function(e)
    {
        //var p = e.target;
        if (this.multiSelect == true)
        {
            var test = this.ID;
            var searchListVisible = $("#searchList" + this.ctrlID).is(":visible");
            if (!searchListVisible)
            {
                var rect = Sys.UI.DomElement.getBounds(this.get_element());
                var h = this.get_maxHeight();
                //check if list goes past window bottom - not flipping above list - just resizing to lesser amount.
                var top = rect.y + rect.height + 2 + this.get_offsetY();
                if (h + top > $(window).height())
                    h = safeSub(h, ((h + top) - $(window).height()) + 5);

                $("#searchListSelection" + this.ctrlID).css("top", top).css("left", rect.x + 2 + this.get_offsetX()).height(h).slideDown(250, function()
                {
                });
            }

            if (this.multiSelect == true && (!this._hideSearchListSelectionDelegate))
            {
                this._hideSearchListSelectionDelegate = Function.createDelegate(this, this.hideSearchListSelection);
                $(document).click(this._hideSearchListSelectionDelegate);
            }
        }
    },

    onSelectionRemoveClick: function(e)
    {
        var p = e.target;
        if (p.tagName == "A" || (p.tagName == "IMG" && p.parentNode.tagName == "A"))
        {
            if (p.tagName == "A")
            {
                searchListControl.searchListMultiSelectOptions = $.grep(searchListControl.searchListMultiSelectOptions, function(value) { return value != p.id; });
                p = p.parentNode;
            }
            if (p.tagName == "IMG")
            {
                searchListControl.searchListMultiSelectOptions = $.grep(searchListControl.searchListMultiSelectOptions, function(value) { return value != p.parentNode.id; });
                p = p.parentNode.parentNode;
            }
            var ph = p.outerHTML;
            $(p).css('display', 'none');

            $('#' + searchListControl.hiddenCtrlID).val(searchListControl.searchListMultiSelectOptions.join(","));
            //Requery search
            //var sl = searchListControl;
            //sl.clear();
        }
    },

    onSelectionAddClick: function(e, a)
    {
        var p = e.target;
        if (p.tagName == "A")
        {
            searchListControl.searchListMultiSelectOptions.push(p.id);

            var ctrlID = [];
            ctrlID = this.id.split("searchList");

            var p = p.parentNode;
            var ph = p.outerHTML;
            $(p).css('display', 'none');

            var h = $("#searchListSelection" + ctrlID[1]).html();
            $("#searchListSelection" + ctrlID[1]).html(h + ph);

            if (ie7)
                $("#searchListSelection" + ctrlID[1]).css("left", $("#searchList" + ctrlID[1]).width() + 23);
            else
                $("#searchListSelection" + ctrlID[1]).css("left", $("#searchList" + ctrlID[1]).width() + 18);


            //Sort
            var list = $("#searchListSelection" + ctrlID[1]);
            var listitems = list.children('div:not(#searchListSelection' + ctrlID[1] + ' div.title1, #searchListSelection' + ctrlID[1] + ' #searchListSelectionHeaderWidth' + ctrlID[1] + ')').get();
            listitems.sort(function(a, b)
            {
                var compA = $(a).text().toUpperCase();
                var compB = $(b).text().toUpperCase();
                return (compA < compB) ? -1 : (compA > compB) ? 1 : 0;
            });


            $.each(listitems, function(idx, itm) { list.append(itm); });

            $('#' + searchListControl.hiddenCtrlID).val(searchListControl.searchListMultiSelectOptions.join(","));
        }
    }
};
Pathfinder.UI.SearchList.registerClass('Pathfinder.UI.SearchList', Sys.UI.Behavior);



if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();





/* Copyright (c) 2007 Paul Bakaus (paul.bakaus@googlemail.com) and Brandon Aaron (brandon.aaron@gmail.com || http://brandonaaron.net)
* Dual licensed under the MIT (http://www.opensource.org/licenses/mit-license.php)
* and GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.
*
* $LastChangedDate: 2007-12-20 08:46:55 -0600 (Thu, 20 Dec 2007) $
* $Rev: 4259 $
*
* Version: 1.2
*
* Requires: jQuery 1.2+
*/

(function($) {

    $.dimensions = {
        version: '1.2'
    };

    // Create innerHeight, innerWidth, outerHeight and outerWidth methods
    $.each(['Height', 'Width'], function(i, name) {

        // innerHeight and innerWidth
        $.fn['inner' + name] = function() {
            if (!this[0]) return;

            var torl = name == 'Height' ? 'Top' : 'Left',  // top or left
		    borr = name == 'Height' ? 'Bottom' : 'Right'; // bottom or right

            return this.is(':visible') ? this[0]['client' + name] : num(this, name.toLowerCase()) + num(this, 'padding' + torl) + num(this, 'padding' + borr);
        };

        // outerHeight and outerWidth
        $.fn['outer' + name] = function(options) {
            if (!this[0]) return;

            var torl = name == 'Height' ? 'Top' : 'Left',  // top or left
		    borr = name == 'Height' ? 'Bottom' : 'Right'; // bottom or right

            options = $.extend({ margin: false }, options || {});

            var val = this.is(':visible') ?
				this[0]['offset' + name] :
				num(this, name.toLowerCase())
					+ num(this, 'border' + torl + 'Width') + num(this, 'border' + borr + 'Width')
					+ num(this, 'padding' + torl) + num(this, 'padding' + borr);

            return val + (options.margin ? (num(this, 'margin' + torl) + num(this, 'margin' + borr)) : 0);
        };
    });

    // Create scrollLeft and scrollTop methods
    $.each(['Left', 'Top'], function(i, name) {
        $.fn['scroll' + name] = function(val) {
            if (!this[0]) return;

            return val != undefined ?

            // Set the scroll offset
			this.each(function() {
			    this == window || this == document ?
					window.scrollTo(
						name == 'Left' ? val : $(window)['scrollLeft'](),
						name == 'Top' ? val : $(window)['scrollTop']()
					) :
					this['scroll' + name] = val;
			}) :

            // Return the scroll offset
			this[0] == window || this[0] == document ?
				self[(name == 'Left' ? 'pageXOffset' : 'pageYOffset')] ||
					$.boxModel && document.documentElement['scroll' + name] ||
					document.body['scroll' + name] :
				this[0]['scroll' + name];
        };
    });

    $.fn.extend({
        position: function() {
            var left = 0, top = 0, elem = this[0], offset, parentOffset, offsetParent, results;

            if (elem) {
                // Get *real* offsetParent
                offsetParent = this.offsetParent();

                // Get correct offsets
                offset = this.offset();
                parentOffset = offsetParent.offset();

                // Subtract element margins
                offset.top -= num(elem, 'marginTop');
                offset.left -= num(elem, 'marginLeft');

                // Add offsetParent borders
                parentOffset.top += num(offsetParent, 'borderTopWidth');
                parentOffset.left += num(offsetParent, 'borderLeftWidth');

                // Subtract the two offsets
                results = {
                    top: offset.top - parentOffset.top,
                    left: offset.left - parentOffset.left
                };
            }

            return results;
        },

        offsetParent: function() {
            var offsetParent = this[0].offsetParent;
            while (offsetParent && (!/^body|html$/i.test(offsetParent.tagName) && $.css(offsetParent, 'position') == 'static'))
                offsetParent = offsetParent.offsetParent;
            return $(offsetParent);
        }
    });

    function num(el, prop) {
        return parseInt($.curCSS(el.jquery ? el[0] : el, prop, true)) || 0;
    };

})(jQuery);

/* 
* JTip
* By Cody Lindley (http://www.codylindley.com)
* Under an Attribution, Share Alike License
* JTip is built on top of the very light weight jquery library.
*/

//on page load (as soon as its ready) call JT_init
//$(document).ready(JT_init);

function JT_init() {
    $("a.jTip")
		   .hover(function() { JT_show(this.href, this.id, this.name) }, function() { $('#JT').remove() })
           .click(function() { return false });
}

function JT_uninit() {
    $("a.jTip").unbind("mouseenter mouseleave click");
}

function JT_show(url, linkId, title) {
    if (title == false) title = "&nbsp;";
    var de = document.documentElement;
    var w = self.innerWidth || (de && de.clientWidth) || document.body.clientWidth;
    var hasArea = w - getAbsoluteLeft(linkId);
    var clickElementy = getAbsoluteTop(linkId) - 3; //set y position

    var queryString = url.replace(/^[^\?]+\??/, '');
    var params = parseQuery(queryString);
    if (params['width'] === undefined) { params['width'] = 600 };
    if (params['link'] !== undefined) {
        $('#' + linkId).bind('click', function() { window.location = params['link'] });
        $('#' + linkId).css('cursor', 'pointer');
    }

    if (hasArea > ((params['width'] * 1) + 75)) {
        $("body").append("<div id='JT' style='width:" + params['width'] * 1 + "px'><div id='JT_arrow_left'></div><div id='JT_close_left'>" + title + "<img class='showHideBtn close' alt='close' title='close' runat='server' src='~/content/images/spacer.gif' onclick='$closeWindow()' /></div><div id='JT_copy'><div class='JT_loader'><div></div></div>"); //right side
        var arrowOffset = getElementWidth(linkId) + 11;
        var clickElementx = getAbsoluteLeft(linkId) + arrowOffset; //set x position
    } else {
        $("body").append("<div id='JT' style='width:" + params['width'] * 1 + "px'><div id='JT_arrow_right' style='left:" + ((params['width'] * 1) + 1) + "px'></div><div id='JT_close_right'>" + title + "</div><div id='JT_copy'><div class='JT_loader'><div></div></div>"); //left side
        var clickElementx = getAbsoluteLeft(linkId) - ((params['width'] * 1) + 15); //set x position
    }

    $('#JT').css({ left: clickElementx + "px", top: clickElementy + "px" });
    $('#JT').show().css("z-index", 99999);
    //$('#JT_copy').load(url);
    clientManager.loadPage(url, "JT_copy");

}

function getElementWidth(objectId) {
    var x = objectId;
    if (typeof (x) == "string")
        x = document.getElementById(objectId);
    return x.offsetWidth;
}

function getAbsoluteLeft(objectId) {
    // Get an object left position from the upper left viewport corner
    var o = objectId;
    if (typeof (o) == "string")
        o = document.getElementById(objectId)
    oLeft = o.offsetLeft            // Get left position from the parent object
    while (o.offsetParent != null) {   // Parse the parent hierarchy up to the document element
        oParent = o.offsetParent    // Get parent object reference
        oLeft += oParent.offsetLeft // Add parent left position
        o = oParent
    }
    return oLeft
}

function getAbsoluteTop(objectId) {
    // Get an object top position from the upper left viewport corner
    var o = objectId;
    if (typeof (o) == "string")
        o = document.getElementById(objectId)
    oTop = o.offsetTop            // Get top position from the parent object
    while (o.offsetParent != null) { // Parse the parent hierarchy up to the document element
        oParent = o.offsetParent  // Get parent object reference
        oTop += oParent.offsetTop // Add parent top position
        o = oParent
    }
    return oTop
}

function parseQuery(query) {
    var Params = new Object();
    if (!query) return Params; // return empty object
    var Pairs = query.split(/[;&]/);
    for (var i = 0; i < Pairs.length; i++) {
        var KeyVal = Pairs[i].split('=');
        if (!KeyVal || KeyVal.length != 2) continue;
        var key = unescape(KeyVal[0]);
        var val = unescape(KeyVal[1]);
        val = val.replace(/\+/g, ' ');
        Params[key] = val;
    }
    return Params;
}

function blockEvents(evt) {
    if (evt.target) {
        evt.preventDefault();
    } else {
        evt.returnValue = false;
    }
}

/*
* jQuery Tooltip plugin 1.3
*
* http://bassistance.de/jquery-plugins/jquery-plugin-tooltip/
* http://docs.jquery.com/Plugins/Tooltip
*
* Copyright (c) 2006 - 2008 Jörn Zaefferer
*
* $Id: jquery.tooltip.js 5741 2008-06-21 15:22:16Z joern.zaefferer $
* 
* Dual licensed under the MIT and GPL licenses:
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
*/

; (function($) {

    // the tooltip element
    var helper = {},
    // the current tooltipped element
		current,
    // the title of the current element, used for restoring
		title,
    // timeout id for delayed tooltips
		tID,
    // IE 5.5 or 6
		IE = $.browser.msie && /MSIE\s(5\.5|6\.)/.test(navigator.userAgent),
    // flag for mouse tracking
		track = false;

    $.tooltip = {
        blocked: false,
        defaults: {
            delay: 200,
            fade: false,
            showURL: true,
            extraClass: "",
            top: 15,
            left: 15,
            id: "tooltip"
        },
        block: function() {
            $.tooltip.blocked = !$.tooltip.blocked;
        }
    };

    $.fn.extend({
        tooltip: function(settings) {
            settings = $.extend({}, $.tooltip.defaults, settings);
            createHelper(settings);
            return this.each(function() {
                $.data(this, "tooltip", settings);
                this.tOpacity = helper.parent.css("opacity");
                // copy tooltip into its own expando and remove the title
                this.tooltipText = this.title;
                $(this).removeAttr("title");
                // also remove alt attribute to prevent default tooltip in IE
                this.alt = "";
            })
				.mouseover(save)
				.mouseout(hide)
				.click(hide);
        },
        fixPNG: IE ? function() {
            return this.each(function() {
                var image = $(this).css('backgroundImage');
                if (image.match(/^url\(["']?(.*\.png)["']?\)$/i)) {
                    image = RegExp.$1;
                    $(this).css({
                        'backgroundImage': 'none',
                        'filter': "progid:DXImageTransform.Microsoft.AlphaImageLoader(enabled=true, sizingMethod=crop, src='" + image + "')"
                    }).each(function() {
                        var position = $(this).css('position');
                        if (position != 'absolute' && position != 'relative')
                            $(this).css('position', 'relative');
                    });
                }
            });
        } : function() { return this; },
        unfixPNG: IE ? function() {
            return this.each(function() {
                $(this).css({ 'filter': '', backgroundImage: '' });
            });
        } : function() { return this; },
        hideWhenEmpty: function() {
            return this.each(function() {
                $(this)[$(this).html() ? "show" : "hide"]();
            });
        },
        url: function() {
            return this.attr('href') || this.attr('src');
        }
    });

    function createHelper(settings) {
        // there can be only one tooltip helper
        if (helper.parent)
            return;
        // create the helper, h3 for title, div for url
        helper.parent = $('<div id="' + settings.id + '"><h3></h3><div class="body"></div><div class="url"></div></div>')
        // add to document
			.appendTo(document.body)
        // hide it at first
			.hide();

        // apply bgiframe if available
        if ($.fn.bgiframe)
            helper.parent.bgiframe();

        // save references to title and url elements
        helper.title = $('h3', helper.parent);
        helper.body = $('div.body', helper.parent);
        helper.url = $('div.url', helper.parent);
    }

    function settings(element) {
        return $.data(element, "tooltip");
    }

    // main event handler to start showing tooltips
    function handle(event) {
        // show helper, either with timeout or on instant
        if (settings(this).delay)
            tID = setTimeout(show, settings(this).delay);
        else
            show();

        // if selected, update the helper position when the mouse moves
        track = !!settings(this).track;
        $(document.body).bind('mousemove', update);

        // update at least once
        update(event);
    }

    // save elements title before the tooltip is displayed
    function save() {
        // if this is the current source, or it has no title (occurs with click event), stop
        if ($.tooltip.blocked || this == current || (!this.tooltipText && !settings(this).bodyHandler))
            return;

        // save current
        current = this;
        title = this.tooltipText;

        if (settings(this).bodyHandler) {
            helper.title.hide();
            var bodyContent = settings(this).bodyHandler.call(this);
            if (bodyContent.nodeType || bodyContent.jquery) {
                helper.body.empty().append(bodyContent)
            } else {
                helper.body.html(bodyContent);
            }
            helper.body.show();
        } else if (settings(this).showBody) {
            var parts = title.split(settings(this).showBody);
            helper.title.html(parts.shift()).show();
            helper.body.empty();
            for (var i = 0, part; (part = parts[i]); i++) {
                if (i > 0)
                    helper.body.append("<br/>");
                helper.body.append(part);
            }
            helper.body.hideWhenEmpty();
        } else {
            helper.title.html(title).show();
            helper.body.hide();
        }

        // if element has href or src, add and show it, otherwise hide it
        if (settings(this).showURL && $(this).url())
            helper.url.html($(this).url().replace('http://', '')).show();
        else
            helper.url.hide();

        // add an optional class for this tip
        helper.parent.addClass(settings(this).extraClass);

        // fix PNG background for IE
        if (settings(this).fixPNG)
            helper.parent.fixPNG();

        handle.apply(this, arguments);
    }

    // delete timeout and show helper
    function show() {
        tID = null;
        if ((!IE || !$.fn.bgiframe) && settings(current).fade) {
            if (helper.parent.is(":animated"))
                helper.parent.stop().show().fadeTo(settings(current).fade, current.tOpacity);
            else
                helper.parent.is(':visible') ? helper.parent.fadeTo(settings(current).fade, current.tOpacity) : helper.parent.fadeIn(settings(current).fade);
        } else {
            helper.parent.show();
        }
        update();
    }

    /**
    * callback for mousemove
    * updates the helper position
    * removes itself when no current element
    */
    function update(event) {
        if ($.tooltip.blocked)
            return;

        if (event && event.target.tagName == "OPTION") {
            return;
        }

        // stop updating when tracking is disabled and the tooltip is visible
        if (!track && helper.parent.is(":visible")) {
            $(document.body).unbind('mousemove', update)
        }

        // if no current element is available, remove this listener
        if (current == null) {
            $(document.body).unbind('mousemove', update);
            return;
        }

        // remove position helper classes
        helper.parent.removeClass("viewport-right").removeClass("viewport-bottom");

        var left = helper.parent[0].offsetLeft;
        var top = helper.parent[0].offsetTop;
        if (event) {
            // position the helper 15 pixel to bottom right, starting from mouse position
            left = event.pageX + settings(current).left;
            top = event.pageY + settings(current).top;
            var right = 'auto';
            if (settings(current).positionLeft) {
                right = $(window).width() - left;
                left = 'auto';
            }
            helper.parent.css({
                left: left,
                right: right,
                top: top
            });
        }

        var v = viewport(),
			h = helper.parent[0];
        // check horizontal position
        if (v.x + v.cx < h.offsetLeft + h.offsetWidth) {
            left -= h.offsetWidth + 20 + settings(current).left;
            helper.parent.css({ left: left + 'px' }).addClass("viewport-right");
        }
        // check vertical position
        if (v.y + v.cy < h.offsetTop + h.offsetHeight) {
            top -= h.offsetHeight + 20 + settings(current).top;
            helper.parent.css({ top: top + 'px' }).addClass("viewport-bottom");
        }
    }

    function viewport() {
        return {
            x: $(window).scrollLeft(),
            y: $(window).scrollTop(),
            cx: $(window).width(),
            cy: $(window).height()
        };
    }

    // hide helper and restore added classes and the title
    function hide(event) {
        if ($.tooltip.blocked)
            return;
        // clear timeout if possible
        if (tID)
            clearTimeout(tID);
        // no more current element
        current = null;

        var tsettings = settings(this);
        function complete() {
            helper.parent.removeClass(tsettings.extraClass).hide().css("opacity", "");
        }
        if ((!IE || !$.fn.bgiframe) && tsettings.fade) {
            if (helper.parent.is(':animated'))
                helper.parent.stop().fadeTo(tsettings.fade, 0, complete);
            else
                helper.parent.stop().fadeOut(tsettings.fade, complete);
        } else
            complete();

        if (settings(this).fixPNG)
            helper.parent.unfixPNG();
    }

})(jQuery);


/*  
===============================================================================
WResize is the jQuery plugin for fixing the IE window resize bug
...............................................................................
Copyright 2007 / Andrea Ercolino
-------------------------------------------------------------------------------
LICENSE: http://www.opensource.org/licenses/mit-license.php
WEBSITE: http://noteslog.com/
===============================================================================
*/

(function($) {
    $.fn.wresize = function(f) {
        version = '1.1';
        wresize = { fired: false, width: 0 };

        function resizeOnce() {
            if ($.browser.msie) {
                if (!wresize.fired) {
                    wresize.fired = true;
                }
                else {
                    var version = parseInt($.browser.version, 10);
                    wresize.fired = false;
                    if (version < 7) {
                        return false;
                    }
                    else if (version == 7) {
                        //a vertical resize is fired once, an horizontal resize twice
                        var width = $(window).width();
                        if (width != wresize.width) {
                            wresize.width = width;
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        function handleWResize(e) {
            if (resizeOnce()) {
                return f.apply(this, [e]);
            }
        }

        this.each(function() {
            if (this == window) {
                $(this).resize(handleWResize);
            }
            else {
                $(this).resize(f);
            }
        });

        return this;
    };

})(jQuery);

////////////////////////////////////////////////////////////////////
/**
* jCarousel - Riding carousels with jQuery
*   http://sorgalla.com/jcarousel/
*
* Copyright (c) 2006 Jan Sorgalla (http://sorgalla.com)
* Dual licensed under the MIT (MIT-LICENSE.txt)
* and GPL (GPL-LICENSE.txt) licenses.
*
* Built on top of the jQuery library
*   http://jquery.com
*
* Inspired by the "Carousel Component" by Bill Scott
*   http://billwscott.com/carousel/
*/

(function($) {
    /**
    * Creates a carousel for all matched elements.
    *
    * @example $("#mycarousel").jcarousel();
    * @before <ul id="mycarousel" class="jcarousel-skin-name"><li>First item</li><li>Second item</li></ul>
    * @result
    *
    * <div class="jcarousel-skin-name">
    *   <div class="jcarousel-container">
    *     <div disabled="disabled" class="jcarousel-prev jcarousel-prev-disabled"></div>
    *     <div class="jcarousel-next"></div>
    *     <div class="jcarousel-clip">
    *       <ul class="jcarousel-list">
    *         <li class="jcarousel-item-1">First item</li>
    *         <li class="jcarousel-item-2">Second item</li>
    *       </ul>
    *     </div>
    *   </div>
    * </div>
    *
    * @name jcarousel
    * @type jQuery
    * @param Hash o A set of key/value pairs to set as configuration properties.
    * @cat Plugins/jCarousel
    */
    $.fn.jcarousel = function(o) {
        return this.each(function() {
            new $jc(this, o);
        });
    };

    // Default configuration properties.
    var defaults = {
        vertical: false,
        start: 1,
        offset: 1,
        size: null,
        scroll: 3,
        visible: null,
        animation: 'normal',
        easing: 'swing',
        auto: 0,
        wrap: null,
        initCallback: null,
        reloadCallback: null,
        itemLoadCallback: null,
        itemFirstInCallback: null,
        itemFirstOutCallback: null,
        itemLastInCallback: null,
        itemLastOutCallback: null,
        itemVisibleInCallback: null,
        itemVisibleOutCallback: null,
        buttonNextHTML: '<div></div>',
        buttonPrevHTML: '<div></div>',
        buttonNextEvent: 'click',
        buttonPrevEvent: 'click',
        buttonNextCallback: null,
        buttonPrevCallback: null
    };

    /**
    * The jCarousel object.
    *
    * @constructor
    * @name $.jcarousel
    * @param Object e The element to create the carousel for.
    * @param Hash o A set of key/value pairs to set as configuration properties.
    * @cat Plugins/jCarousel
    */
    $.jcarousel = function(e, o) {
        this.options = $.extend({}, defaults, o || {});

        this.locked = false;

        this.container = null;
        this.clip = null;
        this.list = null;
        this.buttonNext = null;
        this.buttonPrev = null;

        this.wh = !this.options.vertical ? 'width' : 'height';
        this.lt = !this.options.vertical ? 'left' : 'top';

        // Extract skin class
        var skin = '', split = e.className.split(' ');

        for (var i = 0; i < split.length; i++) {
            if (split[i].indexOf('jcarousel-skin') != -1) {
                $(e).removeClass(split[i]);
                var skin = split[i];
                break;
            }
        }

        if (e.nodeName == 'UL' || e.nodeName == 'OL') {
            this.list = $(e);
            this.container = this.list.parent();

            if (this.container.hasClass('jcarousel-clip')) {
                if (!this.container.parent().hasClass('jcarousel-container'))
                    this.container = this.container.wrap('<div></div>');

                this.container = this.container.parent();
            } else if (!this.container.hasClass('jcarousel-container'))
                this.container = this.list.wrap('<div></div>').parent();
        } else {
            this.container = $(e);
            this.list = $(e).find('>ul,>ol,div>ul,div>ol');
        }

        if (skin != '' && this.container.parent()[0].className.indexOf('jcarousel-skin') == -1)
            this.container.wrap('<div class=" ' + skin + '"></div>');

        this.clip = this.list.parent();

        if (!this.clip.length || !this.clip.hasClass('jcarousel-clip'))
            this.clip = this.list.wrap('<div></div>').parent();

        this.buttonPrev = $('.jcarousel-prev', this.container);

        if (this.buttonPrev.size() == 0 && this.options.buttonPrevHTML != null)
            this.buttonPrev = this.clip.before(this.options.buttonPrevHTML).prev();

        this.buttonPrev.addClass(this.className('jcarousel-prev'));

        this.buttonNext = $('.jcarousel-next', this.container);

        if (this.buttonNext.size() == 0 && this.options.buttonNextHTML != null)
            this.buttonNext = this.clip.before(this.options.buttonNextHTML).prev();

        this.buttonNext.addClass(this.className('jcarousel-next'));

        this.clip.addClass(this.className('jcarousel-clip'));
        this.list.addClass(this.className('jcarousel-list'));
        this.container.addClass(this.className('jcarousel-container'));

        var di = this.options.visible != null ? Math.ceil(this.clipping() / this.options.visible) : null;
        var li = this.list.children('li');

        var self = this;

        if (li.size() > 0) {
            var wh = 0, i = this.options.offset;
            li.each(function() {
                self.format(this, i++);
                wh += self.dimension(this, di);
            });

            //IE 6 Fix - displays last carousel item
            if (/MSIE (\d+\.\d+);/.test(navigator.userAgent))
                var ieversion = new Number(RegExp.$1)

            if (ieversion <= 6)
                wh += 200;
            //END IE 6 Fix

            this.list.css(this.wh, wh + 'px');

            // Only set if not explicitly passed as option
            if (!o || o.size === undefined)
                this.options.size = li.size();
        }

        // For whatever reason, .show() does not work in Safari...
        this.container.css('display', 'block');
        this.buttonNext.css('display', 'block');
        this.buttonPrev.css('display', 'block');

        this.funcNext = function() { self.next(); };
        this.funcPrev = function() { self.prev(); };
        this.funcResize = function() { self.reload(); };

        if (this.options.initCallback != null)
            this.options.initCallback(this, 'init');

        /*if ($.browser.safari)
        {
        this.buttons(false, false);
        $(window).bind('load', function() { self.setup(); });
        } else
        this.setup();*/

        //Chrome Fix
        this.setup();
    };

    // Create shortcut for internal use
    var $jc = $.jcarousel;

    $jc.fn = $jc.prototype = {
        jcarousel: '0.2.3'
    };

    $jc.fn.extend = $jc.extend = $.extend;

    $jc.fn.extend({
        /**
        * Setups the carousel.
        *
        * @name setup
        * @type undefined
        * @cat Plugins/jCarousel
        */
        setup: function() {
            this.first = null;
            this.last = null;
            this.prevFirst = null;
            this.prevLast = null;
            this.animating = false;
            this.timer = null;
            this.tail = null;
            this.inTail = false;

            if (this.locked)
                return;

            this.list.css(this.lt, this.pos(this.options.offset) + 'px');
            var p = this.pos(this.options.start);
            this.prevFirst = this.prevLast = null;
            this.animate(p, false);

            $(window).unbind('resize', this.funcResize).bind('resize', this.funcResize);
        },

        /**
        * Clears the list and resets the carousel.
        *
        * @name reset
        * @type undefined
        * @cat Plugins/jCarousel
        */
        reset: function() {
            this.list.empty();

            this.list.css(this.lt, '0px');
            this.list.css(this.wh, '10px');

            if (this.options.initCallback != null)
                this.options.initCallback(this, 'reset');

            this.setup();
        },

        /**
        * Reloads the carousel and adjusts positions.
        *
        * @name reload
        * @type undefined
        * @cat Plugins/jCarousel
        */
        reload: function() {
            if (this.tail != null && this.inTail)
                this.list.css(this.lt, $jc.intval(this.list.css(this.lt)) + this.tail);

            this.tail = null;
            this.inTail = false;

            if (this.options.reloadCallback != null)
                this.options.reloadCallback(this);

            if (this.options.visible != null) {
                var self = this;
                var di = Math.ceil(this.clipping() / this.options.visible), wh = 0, lt = 0;
                $('li', this.list).each(function(i) {
                    wh += self.dimension(this, di);
                    if (i + 1 < self.first)
                        lt = wh;
                });

                this.list.css(this.wh, wh + 'px');
                this.list.css(this.lt, -lt + 'px');
            }

            this.scroll(this.first, false);
        },

        /**
        * Locks the carousel.
        *
        * @name lock
        * @type undefined
        * @cat Plugins/jCarousel
        */
        lock: function() {
            this.locked = true;
            this.buttons();
        },

        /**
        * Unlocks the carousel.
        *
        * @name unlock
        * @type undefined
        * @cat Plugins/jCarousel
        */
        unlock: function() {
            this.locked = false;
            this.buttons();
        },

        /**
        * Sets the size of the carousel.
        *
        * @name size
        * @type undefined
        * @param Number s The size of the carousel.
        * @cat Plugins/jCarousel
        */
        size: function(s) {
            if (s != undefined) {
                this.options.size = s;
                if (!this.locked)
                    this.buttons();
            }

            return this.options.size;
        },

        /**
        * Checks whether a list element exists for the given index (or index range).
        *
        * @name get
        * @type bool
        * @param Number i The index of the (first) element.
        * @param Number i2 The index of the last element.
        * @cat Plugins/jCarousel
        */
        has: function(i, i2) {
            if (i2 == undefined || !i2)
                i2 = i;

            if (this.options.size !== null && i2 > this.options.size)
                i2 = this.options.size;

            for (var j = i; j <= i2; j++) {
                var e = this.get(j);
                if (!e.length || e.hasClass('jcarousel-item-placeholder'))
                    return false;
            }

            return true;
        },

        /**
        * Returns a jQuery object with list element for the given index.
        *
        * @name get
        * @type jQuery
        * @param Number i The index of the element.
        * @cat Plugins/jCarousel
        */
        get: function(i) {
            return $('.jcarousel-item-' + i, this.list);
        },

        /**
        * Adds an element for the given index to the list.
        * If the element already exists, it updates the inner html.
        * Returns the created element as jQuery object.
        *
        * @name add
        * @type jQuery
        * @param Number i The index of the element.
        * @param String s The innerHTML of the element.
        * @cat Plugins/jCarousel
        */
        add: function(i, s) {
            var e = this.get(i), old = 0, add = 0;

            if (e.length == 0) {
                var c, e = this.create(i), j = $jc.intval(i);
                while (c = this.get(--j)) {
                    if (j <= 0 || c.length) {
                        j <= 0 ? this.list.prepend(e) : c.after(e);
                        break;
                    }
                }
            } else
                old = this.dimension(e);

            e.removeClass(this.className('jcarousel-item-placeholder'));
            typeof s == 'string' ? e.html(s) : e.empty().append(s);

            var di = this.options.visible != null ? Math.ceil(this.clipping() / this.options.visible) : null;
            var wh = this.dimension(e, di) - old;

            if (i > 0 && i < this.first)
                this.list.css(this.lt, $jc.intval(this.list.css(this.lt)) - wh + 'px');

            this.list.css(this.wh, $jc.intval(this.list.css(this.wh)) + wh + 'px');

            return e;
        },

        /**
        * Removes an element for the given index from the list.
        *
        * @name remove
        * @type undefined
        * @param Number i The index of the element.
        * @cat Plugins/jCarousel
        */
        remove: function(i) {
            var e = this.get(i);

            // Check if item exists and is not currently visible
            if (!e.length || (i >= this.first && i <= this.last))
                return;

            var d = this.dimension(e);

            if (i < this.first)
                this.list.css(this.lt, $jc.intval(this.list.css(this.lt)) + d + 'px');

            e.remove();

            this.list.css(this.wh, $jc.intval(this.list.css(this.wh)) - d + 'px');
        },

        /**
        * Moves the carousel forwards.
        *
        * @name next
        * @type undefined
        * @cat Plugins/jCarousel
        */
        next: function() {
            this.stopAuto();

            if (this.tail != null && !this.inTail)
                this.scrollTail(false);
            else
                this.scroll(((this.options.wrap == 'both' || this.options.wrap == 'last') && this.options.size != null && this.last == this.options.size) ? 1 : this.first + this.options.scroll);
        },

        /**
        * Moves the carousel backwards.
        *
        * @name prev
        * @type undefined
        * @cat Plugins/jCarousel
        */
        prev: function() {
            this.stopAuto();

            if (this.tail != null && this.inTail)
                this.scrollTail(true);
            else
                this.scroll(((this.options.wrap == 'both' || this.options.wrap == 'first') && this.options.size != null && this.first == 1) ? this.options.size : this.first - this.options.scroll);
        },

        /**
        * Scrolls the tail of the carousel.
        *
        * @name scrollTail
        * @type undefined
        * @param Bool b Whether scroll the tail back or forward.
        * @cat Plugins/jCarousel
        */
        scrollTail: function(b) {
            if (this.locked || this.animating || !this.tail)
                return;

            var pos = $jc.intval(this.list.css(this.lt));

            !b ? pos -= this.tail : pos += this.tail;
            this.inTail = !b;

            // Save for callbacks
            this.prevFirst = this.first;
            this.prevLast = this.last;

            this.animate(pos);
        },

        /**
        * Scrolls the carousel to a certain position.
        *
        * @name scroll
        * @type undefined
        * @param Number i The index of the element to scoll to.
        * @param Bool a Flag indicating whether to perform animation.
        * @cat Plugins/jCarousel
        */
        scroll: function(i, a) {
            if (this.locked || this.animating)
                return;

            this.animate(this.pos(i), a);
        },

        /**
        * Prepares the carousel and return the position for a certian index.
        *
        * @name pos
        * @type Number
        * @param Number i The index of the element to scoll to.
        * @cat Plugins/jCarousel
        */
        pos: function(i) {
            if (this.locked || this.animating)
                return;

            i = $jc.intval(i);
            if (this.options.wrap != 'circular')
                i = i < 1 ? 1 : (this.options.size && i > this.options.size ? this.options.size : i);

            var back = this.first > i;
            var pos = $jc.intval(this.list.css(this.lt));

            // Create placeholders, new list width/height
            // and new list position
            var f = this.options.wrap != 'circular' && this.first <= 1 ? 1 : this.first;
            var c = back ? this.get(f) : this.get(this.last);
            var j = back ? f : f - 1;
            var e = null, l = 0, p = false, d = 0;

            while (back ? --j >= i : ++j < i) {
                e = this.get(j);
                p = !e.length;
                if (e.length == 0) {
                    e = this.create(j).addClass(this.className('jcarousel-item-placeholder'));
                    c[back ? 'before' : 'after'](e);
                }

                c = e;
                d = this.dimension(e);

                if (p)
                    l += d;

                if (this.first != null && (this.options.wrap == 'circular' || (j >= 1 && (this.options.size == null || j <= this.options.size))))
                    pos = back ? pos + d : pos - d;
            }

            // Calculate visible items
            var clipping = this.clipping();
            var cache = [];
            var visible = 0, j = i, v = 0;
            var c = this.get(i - 1);

            while (++visible) {
                e = this.get(j);
                p = !e.length;
                if (e.length == 0) {
                    e = this.create(j).addClass(this.className('jcarousel-item-placeholder'));
                    // This should only happen on a next scroll
                    c.length == 0 ? this.list.prepend(e) : c[back ? 'before' : 'after'](e);
                }

                c = e;
                var d = this.dimension(e);
                if (d == 0) {
                    //alert('jCarousel: No width/height set for items. This will cause an infinite loop. Aborting...');
                    return 0;
                }

                if (this.options.wrap != 'circular' && this.options.size !== null && j > this.options.size)
                    cache.push(e);
                else if (p)
                    l += d;

                v += d;

                if (v >= clipping)
                    break;

                j++;
            }

            // Remove out-of-range placeholders
            for (var x = 0; x < cache.length; x++)
                cache[x].remove();

            // Resize list
            if (l > 0) {
                this.list.css(this.wh, this.dimension(this.list) + l + 'px');

                if (back) {
                    pos -= l;
                    this.list.css(this.lt, $jc.intval(this.list.css(this.lt)) - l + 'px');
                }
            }

            // Calculate first and last item
            var last = i + visible - 1;
            if (this.options.wrap != 'circular' && this.options.size && last > this.options.size)
                last = this.options.size;

            if (j > last) {
                visible = 0, j = last, v = 0;
                while (++visible) {
                    var e = this.get(j--);
                    if (!e.length)
                        break;
                    v += this.dimension(e);
                    if (v >= clipping)
                        break;
                }
            }

            var first = last - visible + 1;
            if (this.options.wrap != 'circular' && first < 1)
                first = 1;

            if (this.inTail && back) {
                pos += this.tail;
                this.inTail = false;
            }

            this.tail = null;
            if (this.options.wrap != 'circular' && last == this.options.size && (last - visible + 1) >= 1) {
                var m = $jc.margin(this.get(last), !this.options.vertical ? 'marginRight' : 'marginBottom');
                if ((v - m) > clipping)
                    this.tail = v - clipping - m;
            }

            // Adjust position
            while (i-- > first)
                pos += this.dimension(this.get(i));

            // Save visible item range
            this.prevFirst = this.first;
            this.prevLast = this.last;
            this.first = first;
            this.last = last;

            return pos;
        },

        /**
        * Animates the carousel to a certain position.
        *
        * @name animate
        * @type undefined
        * @param mixed p Position to scroll to.
        * @param Bool a Flag indicating whether to perform animation.
        * @cat Plugins/jCarousel
        */
        animate: function(p, a) {
            if (this.locked || this.animating)
                return;

            this.animating = true;

            var self = this;
            var scrolled = function() {
                self.animating = false;

                if (p == 0)
                    self.list.css(self.lt, 0);

                if (self.options.wrap == 'both' || self.options.wrap == 'last' || self.options.size == null || self.last < self.options.size)
                    self.startAuto();

                self.buttons();
                self.notify('onAfterAnimation');
            };

            this.notify('onBeforeAnimation');

            // Animate
            if (!this.options.animation || a == false) {
                this.list.css(this.lt, p + 'px');
                scrolled();
            } else {
                var o = !this.options.vertical ? { 'left': p} : { 'top': p };
                this.list.animate(o, this.options.animation, this.options.easing, scrolled);
            }
        },

        /**
        * Starts autoscrolling.
        *
        * @name auto
        * @type undefined
        * @param Number s Seconds to periodically autoscroll the content.
        * @cat Plugins/jCarousel
        */
        startAuto: function(s) {
            if (s != undefined)
                this.options.auto = s;

            if (this.options.auto == 0)
                return this.stopAuto();

            if (this.timer != null)
                return;

            var self = this;
            this.timer = setTimeout(function() { self.next(); }, this.options.auto * 1000);
        },

        /**
        * Stops autoscrolling.
        *
        * @name stopAuto
        * @type undefined
        * @cat Plugins/jCarousel
        */
        stopAuto: function() {
            if (this.timer == null)
                return;

            clearTimeout(this.timer);
            this.timer = null;
        },

        /**
        * Sets the states of the prev/next buttons.
        *
        * @name buttons
        * @type undefined
        * @cat Plugins/jCarousel
        */
        buttons: function(n, p) {
            if (n == undefined || n == null) {
                var n = !this.locked && this.options.size !== 0 && ((this.options.wrap && this.options.wrap != 'first') || this.options.size == null || this.last < this.options.size);
                if (!this.locked && (!this.options.wrap || this.options.wrap == 'first') && this.options.size != null && this.last >= this.options.size)
                    n = this.tail != null && !this.inTail;
            }

            if (p == undefined || p == null) {
                var p = !this.locked && this.options.size !== 0 && ((this.options.wrap && this.options.wrap != 'last') || this.first > 1);
                if (!this.locked && (!this.options.wrap || this.options.wrap == 'last') && this.options.size != null && this.first == 1)
                    p = this.tail != null && this.inTail;
            }

            var self = this;

            this.buttonNext[n ? 'bind' : 'unbind'](this.options.buttonNextEvent, this.funcNext)[n ? 'removeClass' : 'addClass'](this.className('jcarousel-next-disabled')).attr('disabled', n ? false : true);
            this.buttonPrev[p ? 'bind' : 'unbind'](this.options.buttonPrevEvent, this.funcPrev)[p ? 'removeClass' : 'addClass'](this.className('jcarousel-prev-disabled')).attr('disabled', p ? false : true);

            if (this.buttonNext.length > 0 && (this.buttonNext[0].jcarouselstate == undefined || this.buttonNext[0].jcarouselstate != n) && this.options.buttonNextCallback != null) {
                this.buttonNext.each(function() { self.options.buttonNextCallback(self, this, n); });
                this.buttonNext[0].jcarouselstate = n;
            }

            if (this.buttonPrev.length > 0 && (this.buttonPrev[0].jcarouselstate == undefined || this.buttonPrev[0].jcarouselstate != p) && this.options.buttonPrevCallback != null) {
                this.buttonPrev.each(function() { self.options.buttonPrevCallback(self, this, p); });
                this.buttonPrev[0].jcarouselstate = p;
            }
        },

        notify: function(evt) {
            var state = this.prevFirst == null ? 'init' : (this.prevFirst < this.first ? 'next' : 'prev');

            // Load items
            this.callback('itemLoadCallback', evt, state);

            if (this.prevFirst !== this.first) {
                this.callback('itemFirstInCallback', evt, state, this.first);
                this.callback('itemFirstOutCallback', evt, state, this.prevFirst);
            }

            if (this.prevLast !== this.last) {
                this.callback('itemLastInCallback', evt, state, this.last);
                this.callback('itemLastOutCallback', evt, state, this.prevLast);
            }

            this.callback('itemVisibleInCallback', evt, state, this.first, this.last, this.prevFirst, this.prevLast);
            this.callback('itemVisibleOutCallback', evt, state, this.prevFirst, this.prevLast, this.first, this.last);
        },

        callback: function(cb, evt, state, i1, i2, i3, i4) {
            if (this.options[cb] == undefined || (typeof this.options[cb] != 'object' && evt != 'onAfterAnimation'))
                return;

            var callback = typeof this.options[cb] == 'object' ? this.options[cb][evt] : this.options[cb];

            if (!$.isFunction(callback))
                return;

            var self = this;

            if (i1 === undefined)
                callback(self, state, evt);
            else if (i2 === undefined)
                this.get(i1).each(function() { callback(self, this, i1, state, evt); });
            else {
                for (var i = i1; i <= i2; i++)
                    if (i !== null && !(i >= i3 && i <= i4))
                    this.get(i).each(function() { callback(self, this, i, state, evt); });
            }
        },

        create: function(i) {
            return this.format('<li></li>', i);
        },

        format: function(e, i) {
            var $e = $(e).addClass(this.className('jcarousel-item')).addClass(this.className('jcarousel-item-' + i));
            $e.attr('jcarouselindex', i);
            return $e;
        },

        className: function(c) {
            return c + ' ' + c + (!this.options.vertical ? '-horizontal' : '-vertical');
        },

        dimension: function(e, d) {
            var el = e.jquery != undefined ? e[0] : e;

            var old = !this.options.vertical ?
                el.offsetWidth + $jc.margin(el, 'marginLeft') + $jc.margin(el, 'marginRight') :
                el.offsetHeight + $jc.margin(el, 'marginTop') + $jc.margin(el, 'marginBottom');

            if (d == undefined || old == d)
                return old;

            var w = !this.options.vertical ?
                d - $jc.margin(el, 'marginLeft') - $jc.margin(el, 'marginRight') :
                d - $jc.margin(el, 'marginTop') - $jc.margin(el, 'marginBottom');

            $(el).css(this.wh, w + 'px');

            return this.dimension(el);
        },

        clipping: function() {
            return !this.options.vertical ?
                this.clip[0].offsetWidth - $jc.intval(this.clip.css('borderLeftWidth')) - $jc.intval(this.clip.css('borderRightWidth')) :
                this.clip[0].offsetHeight - $jc.intval(this.clip.css('borderTopWidth')) - $jc.intval(this.clip.css('borderBottomWidth'));
        },

        index: function(i, s) {
            if (s == undefined)
                s = this.options.size;

            return Math.round((((i - 1) / s) - Math.floor((i - 1) / s)) * s) + 1;
        }
    });

    $jc.extend({
        /**
        * Gets/Sets the global default configuration properties.
        *
        * @name defaults
        * @descr Gets/Sets the global default configuration properties.
        * @type Hash
        * @param Hash d A set of key/value pairs to set as configuration properties.
        * @cat Plugins/jCarousel
        */
        defaults: function(d) {
            return $.extend(defaults, d || {});
        },

        margin: function(e, p) {
            if (!e)
                return 0;

            var el = e.jquery != undefined ? e[0] : e;

            if (p == 'marginRight' && $.browser.safari) {
                var old = { 'display': 'block', 'float': 'none', 'width': 'auto' }, oWidth, oWidth2;

                $.swap(el, old, function() { oWidth = el.offsetWidth; });

                old['marginRight'] = 0;
                $.swap(el, old, function() { oWidth2 = el.offsetWidth; });

                return oWidth2 - oWidth;
            }

            return $jc.intval($.css(el, p));
        },

        intval: function(v) {
            v = parseInt(v);
            return isNaN(v) ? 0 : v;
        }
    });

})(jQuery);

/*	
Watermark plugin for jQuery
Version: 3.1.3
http://jquery-watermark.googlecode.com/

Copyright (c) 2009-2011 Todd Northrop
http://www.speednet.biz/
	
March 22, 2011

Requires:  jQuery 1.2.3+
	
Dual licensed under the MIT or GPL Version 2 licenses.
See mit-license.txt and gpl2-license.txt in the project root for details.
------------------------------------------------------*/

(function($, window, undefined) {

    var 
    // String constants for data names
	dataFlag = "watermark",
	dataClass = "watermarkClass",
	dataFocus = "watermarkFocus",
	dataFormSubmit = "watermarkSubmit",
	dataMaxLen = "watermarkMaxLength",
	dataPassword = "watermarkPassword",
	dataText = "watermarkText",

    // Copy of native jQuery regex use to strip return characters from element value
	rreturn = /\r/g,

    // Includes only elements with watermark defined
	selWatermarkDefined = "input:data(" + dataFlag + "),textarea:data(" + dataFlag + ")",

    // Includes only elements capable of having watermark
	selWatermarkAble = "input:text,input:password,input[type=search],input:not([type]),textarea",

    // triggerFns:
    // Array of function names to look for in the global namespace.
    // Any such functions found will be hijacked to trigger a call to
    // hideAll() any time they are called.  The default value is the
    // ASP.NET function that validates the controls on the page
    // prior to a postback.
    // 
    // Am I missing other important trigger function(s) to look for?
    // Please leave me feedback:
    // http://code.google.com/p/jquery-watermark/issues/list
	triggerFns = [
		"Page_ClientValidate"
	],

    // Holds a value of true if a watermark was displayed since the last
    // hideAll() was executed. Avoids repeatedly calling hideAll().
	pageDirty = false,

    // Detects if the browser can handle native placeholders
	hasNativePlaceholder = ("placeholder" in document.createElement("input"));

    // Best practice: this plugin adds only one method to the jQuery object.
    // Also ensures that the watermark code is only added once.
    $.watermark = $.watermark || {

        // Current version number of the plugin
        version: "3.1.3",

        runOnce: true,

        // Default options used when watermarks are instantiated.
        // Can be changed to affect the default behavior for all
        // new or updated watermarks.
        options: {

            // Default class name for all watermarks
            className: "watermark",

            // If true, plugin will detect and use native browser support for
            // watermarks, if available. (e.g., WebKit's placeholder attribute.)
            useNative: true,

            // If true, all watermarks will be hidden during the window's
            // beforeunload event. This is done mainly because WebKit
            // browsers remember the watermark text during navigation
            // and try to restore the watermark text after the user clicks
            // the Back button. We can avoid this by hiding the text before
            // the browser has a chance to save it. The regular unload event
            // was tried, but it seems the browser saves the text before
            // that event kicks off, because it didn't work.
            hideBeforeUnload: true
        },

        // Hide one or more watermarks by specifying any selector type
        // i.e., DOM element, string selector, jQuery matched set, etc.
        hide: function(selector) {
            $(selector).filter(selWatermarkDefined).each(
			function() {
			    $.watermark._hide($(this));
			}
		);
        },

        // Internal use only.
        _hide: function($input, focus) {
            var elem = $input[0],
			inputVal = (elem.value || "").replace(rreturn, ""),
			inputWm = $input.data(dataText) || "",
			maxLen = $input.data(dataMaxLen) || 0,
			className = $input.data(dataClass);

            if ((inputWm.length) && (inputVal == inputWm)) {
                elem.value = "";

                // Password type?
                if ($input.data(dataPassword)) {

                    if (($input.attr("type") || "") === "text") {
                        var $pwd = $input.data(dataPassword) || [],
						$wrap = $input.parent() || [];

                        if (($pwd.length) && ($wrap.length)) {
                            $wrap[0].removeChild($input[0]); // Can't use jQuery methods, because they destroy data
                            $wrap[0].appendChild($pwd[0]);
                            $input = $pwd;
                        }
                    }
                }

                if (maxLen) {
                    $input.attr("maxLength", maxLen);
                    $input.removeData(dataMaxLen);
                }

                if (focus) {
                    $input.attr("autocomplete", "off");  // Avoid NS_ERROR_XPC_JS_THREW_STRING error in Firefox

                    window.setTimeout(
					function() {
					    $input.select();  // Fix missing cursor in IE
					}
				, 1);
                }
            }

            className && $input.removeClass(className);
        },

        // Display one or more watermarks by specifying any selector type
        // i.e., DOM element, string selector, jQuery matched set, etc.
        // If conditions are not right for displaying a watermark, ensures that watermark is not shown.
        show: function(selector) {
            $(selector).filter(selWatermarkDefined).each(
			function() {
			    $.watermark._show($(this));
			}
		);
        },

        // Internal use only.
        _show: function($input) {
            var elem = $input[0],
			val = (elem.value || "").replace(rreturn, ""),
			text = $input.data(dataText) || "",
			type = $input.attr("type") || "",
			className = $input.data(dataClass);

            if (((val.length == 0) || (val == text)) && (!$input.data(dataFocus))) {
                pageDirty = true;

                // Password type?
                if ($input.data(dataPassword)) {

                    if (type === "password") {
                        var $pwd = $input.data(dataPassword) || [],
						$wrap = $input.parent() || [];

                        if (($pwd.length) && ($wrap.length)) {
                            $wrap[0].removeChild($input[0]); // Can't use jQuery methods, because they destroy data
                            $wrap[0].appendChild($pwd[0]);
                            $input = $pwd;
                            $input.attr("maxLength", text.length);
                            elem = $input[0];
                        }
                    }
                }

                // Ensure maxLength big enough to hold watermark (input of type="text" or type="search" only)
                if ((type === "text") || (type === "search")) {
                    var maxLen = $input.attr("maxLength") || 0;

                    if ((maxLen > 0) && (text.length > maxLen)) {
                        $input.data(dataMaxLen, maxLen);
                        $input.attr("maxLength", text.length);
                    }
                }

                className && $input.addClass(className);
                elem.value = text;
            }
            else {
                $.watermark._hide($input);
            }
        },

        // Hides all watermarks on the current page.
        hideAll: function() {
            if (pageDirty) {
                $.watermark.hide(selWatermarkAble);
                pageDirty = false;
            }
        },

        // Displays all watermarks on the current page.
        showAll: function() {
            $.watermark.show(selWatermarkAble);
        }
    };

    $.fn.watermark = $.fn.watermark || function(text, options) {
        ///	<summary>
        ///		Set watermark text and class name on all input elements of type="text/password/search" and
        /// 	textareas within the matched set. If className is not specified in options, the default is
        /// 	"watermark". Within the matched set, only input elements with type="text/password/search"
        /// 	and textareas are affected; all other elements are ignored.
        ///	</summary>
        ///	<returns type="jQuery">
        ///		Returns the original jQuery matched set (not just the input and texarea elements).
        /// </returns>
        ///	<param name="text" type="String">
        ///		Text to display as a watermark when the input or textarea element has an empty value and does not
        /// 	have focus. The first time watermark() is called on an element, if this argument is empty (or not
        /// 	a String type), then the watermark will have the net effect of only changing the class name when
        /// 	the input or textarea element's value is empty and it does not have focus.
        ///	</param>
        ///	<param name="options" type="Object" optional="true">
        ///		Provides the ability to override the default watermark options ($.watermark.options). For backward
        /// 	compatibility, if a string value is supplied, it is used as the class name that overrides the class
        /// 	name in $.watermark.options.className. Properties include:
        /// 		className: When the watermark is visible, the element will be styled using this class name.
        /// 		useNative (Boolean or Function): Specifies if native browser support for watermarks will supersede
        /// 			plugin functionality. If useNative is a function, the return value from the function will
        /// 			determine if native support is used. The function is passed one argument -- a jQuery object
        /// 			containing the element being tested as the only element in its matched set -- and the DOM
        /// 			element being tested is the object on which the function is invoked (the value of "this").
        ///	</param>
        /// <remarks>
        ///		The effect of changing the text and class name on an input element is called a watermark because
        ///		typically light gray text is used to provide a hint as to what type of input is required. However,
        ///		the appearance of the watermark can be something completely different: simply change the CSS style
        ///		pertaining to the supplied class name.
        ///		
        ///		The first time watermark() is called on an element, the watermark text and class name are initialized,
        ///		and the focus and blur events are hooked in order to control the display of the watermark.  Also, as
        /// 	of version 3.0, drag and drop events are hooked to guard against dropped text being appended to the
        /// 	watermark.  If native watermark support is provided by the browser, it is detected and used, unless
        /// 	the useNative option is set to false.
        ///		
        ///		Subsequently, watermark() can be called again on an element in order to change the watermark text
        ///		and/or class name, and it can also be called without any arguments in order to refresh the display.
        ///		
        ///		For example, after changing the value of the input or textarea element programmatically, watermark()
        /// 	should be called without any arguments to refresh the display, because the change event is only
        /// 	triggered by user actions, not by programmatic changes to an input or textarea element's value.
        /// 	
        /// 	The one exception to programmatic updates is for password input elements:  you are strongly cautioned
        /// 	against changing the value of a password input element programmatically (after the page loads).
        /// 	The reason is that some fairly hairy code is required behind the scenes to make the watermarks bypass
        /// 	IE security and switch back and forth between clear text (for watermarks) and obscured text (for
        /// 	passwords).  It is *possible* to make programmatic changes, but it must be done in a certain way, and
        /// 	overall it is not recommended.
        /// </remarks>

        if (!this.length) {
            return this;
        }

        var hasClass = false,
		hasText = (typeof (text) === "string");

        if (hasText) {
            text = text.replace(rreturn, "");
        }

        if (typeof (options) === "object") {
            hasClass = (typeof (options.className) === "string");
            options = $.extend({}, $.watermark.options, options);
        }
        else if (typeof (options) === "string") {
            hasClass = true;
            options = $.extend({}, $.watermark.options, { className: options });
        }
        else {
            options = $.watermark.options;
        }

        if (typeof (options.useNative) !== "function") {
            options.useNative = options.useNative ? function() { return true; } : function() { return false; };
        }

        return this.each(
		function() {
		    var $input = $(this);

		    if (!$input.is(selWatermarkAble)) {
		        return;
		    }

		    // Watermark already initialized?
		    if ($input.data(dataFlag)) {

		        // If re-defining text or class, first remove existing watermark, then make changes
		        if (hasText || hasClass) {
		            $.watermark._hide($input);

		            if (hasText) {
		                $input.data(dataText, text);
		            }

		            if (hasClass) {
		                $input.data(dataClass, options.className);
		            }
		        }
		    }
		    else {

		        // Detect and use native browser support, if enabled in options
		        if (
					(hasNativePlaceholder)
					&& (options.useNative.call(this, $input))
					&& (($input.attr("tagName") || "") !== "TEXTAREA")
				) {
		            // className is not set because current placeholder standard doesn't
		            // have a separate class name property for placeholders (watermarks).
		            if (hasText) {
		                $input.attr("placeholder", text);
		            }

		            // Only set data flag for non-native watermarks
		            // [purposely commented-out] -> $input.data(dataFlag, 1);
		            return;
		        }

		        $input.data(dataText, hasText ? text : "");
		        $input.data(dataClass, options.className);
		        $input.data(dataFlag, 1); // Flag indicates watermark was initialized

		        // Special processing for password type
		        if (($input.attr("type") || "") === "password") {
		            var $wrap = $input.wrap("<span>").parent(),
						$wm = $($wrap.html().replace(/type=["']?password["']?/i, 'type="text"'));

		            $wm.data(dataText, $input.data(dataText));
		            $wm.data(dataClass, $input.data(dataClass));
		            $wm.data(dataFlag, 1);
		            $wm.attr("maxLength", text.length);

		            $wm.focus(
						function() {
						    $.watermark._hide($wm, true);
						}
					).bind("dragenter",
						function() {
						    $.watermark._hide($wm);
						}
					).bind("dragend",
						function() {
						    window.setTimeout(function() { $wm.blur(); }, 1);
						}
					);
		            $input.blur(
						function() {
						    $.watermark._show($input);
						}
					).bind("dragleave",
						function() {
						    $.watermark._show($input);
						}
					);

		            $wm.data(dataPassword, $input);
		            $input.data(dataPassword, $wm);
		        }
		        else {

		            $input.focus(
						function() {
						    $input.data(dataFocus, 1);
						    $.watermark._hide($input, true);
						}
					).blur(
						function() {
						    $input.data(dataFocus, 0);
						    $.watermark._show($input);
						}
					).bind("dragenter",
						function() {
						    $.watermark._hide($input);
						}
					).bind("dragleave",
						function() {
						    $.watermark._show($input);
						}
					).bind("dragend",
						function() {
						    window.setTimeout(function() { $.watermark._show($input); }, 1);
						}
					).bind("drop",
		            // Firefox makes this lovely function necessary because the dropped text
		            // is merged with the watermark before the drop event is called.
						function(evt) {
						    var elem = $input[0],
								dropText = evt.originalEvent.dataTransfer.getData("Text");

						    if ((elem.value || "").replace(rreturn, "").replace(dropText, "") === $input.data(dataText)) {
						        elem.value = dropText;
						    }

						    $input.focus();
						}
					);
		        }

		        // In order to reliably clear all watermarks before form submission,
		        // we need to replace the form's submit function with our own
		        // function.  Otherwise watermarks won't be cleared when the form
		        // is submitted programmatically.
		        if (this.form) {
		            var form = this.form,
						$form = $(form);

		            if (!$form.data(dataFormSubmit)) {
		                $form.submit($.watermark.hideAll);

		                // form.submit exists for all browsers except Google Chrome
		                // (see "else" below for explanation)
		                if (form.submit) {
		                    $form.data(dataFormSubmit, form.submit);

		                    form.submit = (function(f, $f) {
		                        return function() {
		                            var nativeSubmit = $f.data(dataFormSubmit);

		                            $.watermark.hideAll();

		                            if (nativeSubmit.apply) {
		                                nativeSubmit.apply(f, Array.prototype.slice.call(arguments));
		                            }
		                            else {
		                                nativeSubmit();
		                            }
		                        };
		                    })(form, $form);
		                }
		                else {
		                    $form.data(dataFormSubmit, 1);

		                    // This strangeness is due to the fact that Google Chrome's
		                    // form.submit function is not visible to JavaScript (identifies
		                    // as "undefined").  I had to invent a solution here because hours
		                    // of Googling (ironically) for an answer did not turn up anything
		                    // useful.  Within my own form.submit function I delete the form's
		                    // submit function, and then call the non-existent function --
		                    // which, in the world of Google Chrome, still exists.
		                    form.submit = (function(f) {
		                        return function() {
		                            $.watermark.hideAll();
		                            delete f.submit;
		                            f.submit();
		                        };
		                    })(form);
		                }
		            }
		        }
		    }

		    $.watermark._show($input);
		}
	);
    };

    // The code included within the following if structure is guaranteed to only run once,
    // even if the watermark script file is included multiple times in the page.
    if ($.watermark.runOnce) {
        $.watermark.runOnce = false;

        $.extend($.expr[":"], {

            // Extends jQuery with a custom selector - ":data(...)"
            // :data(<name>)  Includes elements that have a specific name defined in the jQuery data
            // collection. (Only the existence of the name is checked; the value is ignored.)
            // A more sophisticated version of the :data() custom selector originally part of this plugin
            // was removed for compatibility with jQuery UI. The original code can be found in the SVN
            // source listing in the file, "jquery.data.js".
            data: function(elem, i, match) {
                return !!$.data(elem, match[3]);
            }
        });

        // Overloads the jQuery .val() function to return the underlying input value on
        // watermarked input elements.  When .val() is being used to set values, this
        // function ensures watermarks are properly set/removed after the values are set.
        // Uses self-executing function to override the default jQuery function.
        (function(valOld) {

            $.fn.val = function() {

                // Best practice: return immediately if empty matched set
                if (!this.length) {
                    return arguments.length ? this : undefined;
                }

                // If no args, then we're getting the value of the first element;
                // otherwise we're setting values for all elements in matched set
                if (!arguments.length) {

                    // If element is watermarked, get the underlying value;
                    // otherwise use native jQuery .val()
                    if (this.data(dataFlag)) {
                        var v = (this[0].value || "").replace(rreturn, "");
                        return (v === (this.data(dataText) || "")) ? "" : v;
                    }
                    else {
                        return valOld.apply(this, arguments);
                    }
                }
                else {
                    valOld.apply(this, arguments);
                    $.watermark.show(this);
                    return this;
                }
            };

        })($.fn.val);

        // Hijack any functions found in the triggerFns list
        if (triggerFns.length) {

            // Wait until DOM is ready before searching
            $(function() {
                var i, name, fn;

                for (i = triggerFns.length - 1; i >= 0; i--) {
                    name = triggerFns[i];
                    fn = window[name];

                    if (typeof (fn) === "function") {
                        window[name] = (function(origFn) {
                            return function() {
                                $.watermark.hideAll();
                                return origFn.apply(null, Array.prototype.slice.call(arguments));
                            };
                        })(fn);
                    }
                }
            });
        }

        $(window).bind("beforeunload", function() {
            if ($.watermark.options.hideBeforeUnload) {
                $.watermark.hideAll();
            }
        });
    }

})(jQuery, window);


