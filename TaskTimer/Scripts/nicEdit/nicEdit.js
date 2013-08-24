/* NicEdit - Micro Inline WYSIWYG
 * Copyright 2007-2008 Brian Kirchoff
 *
 * NicEdit is distributed under the terms of the MIT license
 * For more information visit http://nicedit.com/
 * Do not remove this copyright message
 */
var bkExtend = function () {
    var A = arguments;
    if (A.length == 1) {
        A = [this, A[0]]
    }
    for (var B in A[1]) {
        A[0][B] = A[1][B]
    }
    return A[0]
};
function bkClass() {
}
bkClass.prototype.construct = function () {
};
bkClass.extend = function (C) {
    var A = function () {
        if (arguments[0] !== bkClass) {
            return this.construct.apply(this, arguments)
        }
    };
    var B = new this(bkClass);
    bkExtend(B, C);
    A.prototype = B;
    A.extend = this.extend;
    return A
};
var bkElement = bkClass.extend({
    construct: function (B, A) {
        if (typeof (B) == "string") {
            B = (A || document).createElement(B)
        }
        B = $BK(B);
        return B
    }, appendTo: function (A) {
        A.appendChild(this);
        return this
    }, appendBefore: function (A) {
        A.parentNode.insertBefore(this, A);
        return this
    }, addEvent: function (B, A) {
        bkLib.addEvent(this, B, A);
        return this
    }, setContent: function (A) {
        this.innerHTML = A;
        return this
    }, pos: function () {
        var C = curtop = 0;
        var B = obj = this;
        if (obj.offsetParent) {
            do {
                C += obj.offsetLeft;
                curtop += obj.offsetTop
            } while (obj = obj.offsetParent)
        }
        var A = (!window.opera) ? parseInt(this.getStyle("border-width") || this.style.border) || 0 : 0;
        return [C + A, curtop + A + this.offsetHeight]
    }, noSelect: function () {
        bkLib.noSelect(this);
        return this
    }, parentTag: function (A) {
        var B = this;
        do {
            if (B && B.nodeName && B.nodeName.toUpperCase() == A) {
                return B
            }
            B = B.parentNode
        } while (B);
        return false
    }, hasClass: function (A) {
        return this.className.match(new RegExp("(\\s|^)nicEdit-" + A + "(\\s|$)"))
    }, addClass: function (A) {
        if (!this.hasClass(A)) {
            this.className += " nicEdit-" + A
        }
        return this
    }, removeClass: function (A) {
        if (this.hasClass(A)) {
            this.className = this.className.replace(new RegExp("(\\s|^)nicEdit-" + A + "(\\s|$)"), " ")
        }
        return this
    }, setStyle: function (A) {
        var B = this.style;
        for (var C in A) {
            switch (C) {
                case "float":
                    B.cssFloat = B.styleFloat = A[C];
                    break;
                case "opacity":
                    B.opacity = A[C];
                    B.filter = "alpha(opacity=" + Math.round(A[C] * 100) + ")";
                    break;
                case "className":
                    this.className = A[C];
                    break;
                default:
                    B[C] = A[C]
            }
        }
        return this
    }, getStyle: function (A, C) {
        var B = (!C) ? document.defaultView : C;
        if (this.nodeType == 1) {
            return (B && B.getComputedStyle) ? B.getComputedStyle(this, null).getPropertyValue(A) : this.currentStyle[bkLib.camelize(A)]
        }
    }, remove: function () {
        this.parentNode.removeChild(this);
        return this
    }, setAttributes: function (A) {
        for (var B in A) {
            this[B] = A[B]
        }
        return this
    }
});
var bkLib = {
    isMSIE: (navigator.appVersion.indexOf("MSIE") != -1), addEvent: function (C, B, A) {
        (C.addEventListener) ? C.addEventListener(B, A, false) : C.attachEvent("on" + B, A)
    }, toArray: function (C) {
        var B = C.length, A = new Array(B);
        while (B--) {
            A[B] = C[B]
        }
        return A
    }, noSelect: function (B) {
        if (B.setAttribute && B.nodeName.toLowerCase() != "input" && B.nodeName.toLowerCase() != "textarea") {
            B.setAttribute("unselectable", "on")
        }
        for (var A = 0; A < B.childNodes.length; A++) {
            bkLib.noSelect(B.childNodes[A])
        }
    }, camelize: function (A) {
        return A.replace(/\-(.)/g, function (B, C) {
            return C.toUpperCase()
        })
    }, inArray: function (A, B) {
        return (bkLib.search(A, B) != null)
    }, search: function (A, C) {
        for (var B = 0; B < A.length; B++) {
            if (A[B] == C) {
                return B
            }
        }
        return null
    }, cancelEvent: function (A) {
        A = A || window.event;
        if (A.preventDefault && A.stopPropagation) {
            A.preventDefault();
            A.stopPropagation()
        }
        return false
    }, domLoad: [], domLoaded: function () {
        if (arguments.callee.done) {
            return
        }
        arguments.callee.done = true;
        for (i = 0; i < bkLib.domLoad.length; i++) {
            bkLib.domLoad[i]()
        }
    }, onDomLoaded: function (A) {
        this.domLoad.push(A);
        if (document.addEventListener) {
            document.addEventListener("DOMContentLoaded", bkLib.domLoaded, null)
        } else {
            if (bkLib.isMSIE) {
                document.write("<style>.nicEdit-main p { margin: 0; }</style><script id=__ie_onload defer " + ((location.protocol == "https:") ? "src='javascript:void(0)'" : "src=//0") + "><\/script>");
                $BK("__ie_onload").onreadystatechange = function () {
                    if (this.readyState == "complete") {
                        bkLib.domLoaded()
                    }
                }
            }
        }
        window.onload = bkLib.domLoaded
    }
};
function $BK(A) {
    if (typeof (A) == "string") {
        A = document.getElementById(A)
    }
    return (A && !A.appendTo) ? bkExtend(A, bkElement.prototype) : A
}
var bkEvent = {
    addEvent: function (A, B) {
        if (B) {
            this.eventList = this.eventList || {};
            this.eventList[A] = this.eventList[A] || [];
            this.eventList[A].push(B)
        }
        return this
    }, fireEvent: function () {
        var A = bkLib.toArray(arguments), C = A.shift();
        if (this.eventList && this.eventList[C]) {
            for (var B = 0; B < this.eventList[C].length; B++) {
                this.eventList[C][B].apply(this, A)
            }
        }
    }
};
function __(A) {
    return A
}
Function.prototype.closure = function () {
    var A = this, B = bkLib.toArray(arguments), C = B.shift();
    return function () {
        if (typeof (bkLib) != "undefined") {
            return A.apply(C, B.concat(bkLib.toArray(arguments)))
        }
    }
};
Function.prototype.closureListener = function () {
    var A = this, C = bkLib.toArray(arguments), B = C.shift();
    return function (E) {
        E = E || window.event;
        if (E.target) {
            var D = E.target
        } else {
            var D = E.srcElement
        }
        return A.apply(B, [E, D].concat(C))
    }
};



var nicEditorConfig = bkClass.extend({
    buttons: {
        'bold': { name: __('Click to Bold'), command: 'Bold', tags: ['B', 'STRONG'], css: { 'font-weight': 'bold' }, key: 'b' },
        'italic': { name: __('Click to Italic'), command: 'Italic', tags: ['EM', 'I'], css: { 'font-style': 'italic' }, key: 'i' },
        'underline': { name: __('Click to Underline'), command: 'Underline', tags: ['U'], css: { 'text-decoration': 'underline' }, key: 'u' },
        'left': { name: __('Left Align'), command: 'justifyleft', noActive: true },
        'center': { name: __('Center Align'), command: 'justifycenter', noActive: true },
        'right': { name: __('Right Align'), command: 'justifyright', noActive: true },
        'justify': { name: __('Justify Align'), command: 'justifyfull', noActive: true },
        'ol': { name: __('Insert Ordered List'), command: 'insertorderedlist', tags: ['OL'] },
        'ul': { name: __('Insert Unordered List'), command: 'insertunorderedlist', tags: ['UL'] },
        'subscript': { name: __('Click to Subscript'), command: 'subscript', tags: ['SUB'] },
        'superscript': { name: __('Click to Superscript'), command: 'superscript', tags: ['SUP'] },
        'strikethrough': { name: __('Click to Strike Through'), command: 'strikeThrough', css: { 'text-decoration': 'line-through' } },
        'removeformat': { name: __('Remove Formatting'), command: 'removeformat', noActive: true },
        'indent': { name: __('Indent Text'), command: 'indent', noActive: true },
        'outdent': { name: __('Remove Indent'), command: 'outdent', noActive: true },
        'hr': { name: __('Horizontal Rule'), command: 'insertHorizontalRule', noActive: true }
    },
    //iconsPath: '/Scripts/nicEdit/nicEditorIcons.gif',
    iconsPath: '/Scripts/nicEdit/nicEditIconsNew.gif',
    
    buttonList: ['save', 'bold', 'italic', 'underline', 'left', 'center', 'right', 'justify', 'ol', 'ul', 'fontSize', 'fontFamily', 'fontFormat', 'indent', 'outdent', 'image', 'upload', 'link', 'unlink', 'forecolor', 'bgcolor', 'xhtml'],
    iconList: { "xhtml": 1, "bgcolor": 2, "forecolor": 3, "bold": 4, "center": 5, "hr": 6, "indent": 7, "italic": 8, "justify": 9, "left": 10, "ol": 11, "outdent": 12, "removeformat": 13, "right": 14, "save": 15, "strikethrough": 16, "subscript": 17, "superscript": 18, "ul": 19, "underline": 20, "image": 21, "link": 22, "unlink": 23, "close": 24, "upload": 25, "arrow": 26 }

});


;
var nicEditors = {
    nicPlugins: [], editors: [], registerPlugin: function (n, t) {
        this.nicPlugins.push({ p: n, o: t })
    }, allTextAreas: function (n) {
        for (var i = document.getElementsByTagName("textarea"), t = 0; t < i.length; t++)
            nicEditors.editors.push(new nicEditor(n).panelInstance(i[t]));
        return nicEditors.editors
    }, findEditor: function (n) {
        for (var i = nicEditors.editors, t = 0; t < i.length; t++)
            if (i[t].instanceById(n))
                return i[t].instanceById(n)
    }
}, nicEditor = bkClass.extend({
    construct: function (n) {
        var i, t;
        for (this.options = new nicEditorConfig, bkExtend(this.options, n), this.nicInstances = [], this.loadedPlugins = [], i = nicEditors.nicPlugins, t = 0; t < i.length; t++)
            this.loadedPlugins.push(new i[t].p(this, i[t].o));
        nicEditors.editors.push(this), bkLib.addEvent(document.body, "mousedown", this.selectCheck.closureListener(this))
    }, panelInstance: function (n, t) {
        n = this.checkReplace($BK(n));
        var i = new bkElement("DIV").setStyle({ width: (parseInt(n.getStyle("width")) || n.clientWidth) + "px" }).appendBefore(n);
        return this.setPanel(i), this.addInstance(n, t)
    }, checkReplace: function (n) {
        var t = nicEditors.findEditor(n);
        return t && (t.removeInstance(n), t.removePanel()), n
    }, addInstance: function (n, t) {
        var i;
        return n = this.checkReplace($BK(n)), i = n.contentEditable || !!window.opera ? new nicEditorInstance(n, t, this) : new nicEditorIFrameInstance(n, t, this), this.nicInstances.push(i), this
    }, removeInstance: function (n) {
        var i, t;
        for (n = $BK(n), i = this.nicInstances, t = 0; t < i.length; t++)
            i[t].e == n && (i[t].remove(), this.nicInstances.splice(t, 1))
    }, removePanel: function () {
        this.nicPanel && (this.nicPanel.remove(), this.nicPanel = null)
    }, instanceById: function (n) {
        var i, t;
        for (n = $BK(n), i = this.nicInstances, t = 0; t < i.length; t++)
            if (i[t].e == n)
                return i[t]
    }, setPanel: function (n) {
        return this.nicPanel = new nicEditorPanel($BK(n), this.options, this), this.fireEvent("panel", this.nicPanel), this
    }, nicCommand: function (n, t) {
        this.selectedInstance && this.selectedInstance.nicCommand(n, t)
    }, getIcon: function (n, t) {
        var i = this.options.iconList[n], r = t.iconFiles ? t.iconFiles[n] : "";
        return { backgroundImage: "url('" + (i ? this.options.iconsPath : r) + "')", backgroundPosition: (i ? (i - 1) * -18 : 0) + "px 0px" }
    }, selectCheck: function (n, t) {
        do
            if (t.className && t.className.indexOf("nicEdit") != -1)
                return !1;
        while (t = t.parentNode);
        return this.fireEvent("blur", this.selectedInstance, t), this.lastSelectedInstance = this.selectedInstance, this.selectedInstance = null, !1
    }
});
nicEditor = nicEditor.extend(bkEvent);
var nicEditorInstance = bkClass.extend({
    isSelected: !1, construct: function (n, t, i) {
        var u, f, e, r, o;
        this.ne = i, this.elm = this.e = n, this.options = t || {}, newX = parseInt(n.getStyle("width")) || n.clientWidth, newY = parseInt(n.getStyle("height")) || n.clientHeight, this.initialHeight = newY - 8, u = n.nodeName.toLowerCase() == "textarea", (u || this.options.hasPanel) && (f = bkLib.isMSIE && !(typeof document.body.style.maxHeight != "undefined" && document.compatMode == "CSS1Compat"), e = { width: newX + "px", border: "1px solid #ccc", borderTop: 0, overflowY: "auto", overflowX: "hidden" }, e[f ? "height" : "maxHeight"] = this.ne.options.maxHeight ? this.ne.options.maxHeight + "px" : null, this.editorContain = new bkElement("DIV").setStyle(e).appendBefore(n), r = new bkElement("DIV").setStyle({ width: newX - 8 + "px", margin: "4px", minHeight: newY + "px" }).addClass("main").appendTo(this.editorContain), n.setStyle({ display: "none" }), r.innerHTML = n.innerHTML, u && (r.setContent(n.value), this.copyElm = n, o = n.parentTag("FORM"), o && bkLib.addEvent(o, "submit", this.saveContent.closure(this))), r.setStyle(f ? { height: newY + "px" } : { overflow: "hidden" }), this.elm = r), this.ne.addEvent("blur", this.blur.closure(this)), this.init(), this.blur()
    }, init: function () {
        this.elm.setAttribute("contentEditable", "true"), this.getContent() == "" && this.setContent("<br />"), this.instanceDoc = document.defaultView, this.elm.addEvent("mousedown", this.selected.closureListener(this)).addEvent("keypress", this.keyDown.closureListener(this)).addEvent("focus", this.selected.closure(this)).addEvent("blur", this.blur.closure(this)).addEvent("keyup", this.selected.closure(this)), this.ne.fireEvent("add", this)
    }, remove: function () {
        this.saveContent(), (this.copyElm || this.options.hasPanel) && (this.editorContain.remove(), this.e.setStyle({ display: "block" }), this.ne.removePanel()), this.disable(), this.ne.fireEvent("remove", this)
    }, disable: function () {
        this.elm.setAttribute("contentEditable", "false")
    }, getSel: function () {
        return window.getSelection ? window.getSelection() : document.selection
    }, getRng: function () {
        var n = this.getSel();
        if (n && n.rangeCount !== 0)
            return n.rangeCount > 0 ? n.getRangeAt(0) : n.createRange()
    }, selRng: function (n, t) {
        window.getSelection ? (t.removeAllRanges(), t.addRange(n)) : n.select()
    }, selElm: function () {
        var n = this.getRng(), t, i, r;
        if (n) {
            if (n.startContainer) {
                if (t = n.startContainer, n.cloneContents().childNodes.length == 1)
                    for (i = 0; i < t.childNodes.length; i++)
                        if (r = t.childNodes[i].ownerDocument.createRange(), r.selectNode(t.childNodes[i]), n.compareBoundaryPoints(Range.START_TO_START, r) != 1 && n.compareBoundaryPoints(Range.END_TO_END, r) != -1)
                            return $BK(t.childNodes[i]);
                return $BK(t)
            }
            return $BK(this.getSel().type == "Control" ? n.item(0) : n.parentElement())
        }
    }, saveRng: function () {
        this.savedRange = this.getRng(), this.savedSel = this.getSel()
    }, restoreRng: function () {
        this.savedRange && this.selRng(this.savedRange, this.savedSel)
    }, keyDown: function (n) {
        n.ctrlKey && this.ne.fireEvent("key", this, n)
    }, selected: function (n, t) {
        if (t || (t = this.selElm) || (t = this.selElm()), !n.ctrlKey) {
            var i = this.ne.selectedInstance;
            i != this && (i && this.ne.fireEvent("blur", i, t), this.ne.selectedInstance = this, this.ne.fireEvent("focus", i, t)), this.ne.fireEvent("selected", i, t), this.isFocused = !0, this.elm.addClass("selected")
        }
        return !1
    }, blur: function () {
        this.isFocused = !1, this.elm.removeClass("selected")
    }, saveContent: function () {
        (this.copyElm || this.options.hasPanel) && (this.ne.fireEvent("save", this), this.copyElm ? this.copyElm.value = this.getContent() : this.e.innerHTML = this.getContent())
    }, getElm: function () {
        return this.elm
    }, getContent: function () {
        return this.content = this.getElm().innerHTML, this.ne.fireEvent("get", this), this.content
    }, setContent: function (n) {
        this.content = n, this.ne.fireEvent("set", this), this.elm.innerHTML = this.content
    }, nicCommand: function (n, t) {
        document.execCommand(n, !1, t)
    }
}), nicEditorIFrameInstance = nicEditorInstance.extend({
    savedStyles: [], init: function () {
        var n = this.elm.innerHTML.replace(/^\s+|\s+$/g, ""), t;
        this.elm.innerHTML = "", n ? n : n = "<br />", this.initialContent = n, this.elmFrame = new bkElement("iframe").setAttributes({ src: "javascript:;", frameBorder: 0, allowTransparency: "true", scrolling: "no" }).setStyle({ height: "100px", width: "100%" }).addClass("frame").appendTo(this.elm), this.copyElm && this.elmFrame.setStyle({ width: this.elm.offsetWidth - 4 + "px" }), t = ["font-size", "font-family", "font-weight", "color"];
        for (itm in t)
            this.savedStyles[bkLib.camelize(itm)] = this.elm.getStyle(itm);
        setTimeout(this.initFrame.closure(this), 50)
    }, disable: function () {
        this.elm.innerHTML = this.getContent()
    }, initFrame: function () {
        var n = $BK(this.elmFrame.contentWindow.document), t;
        n.designMode = "on", n.open(), t = this.ne.options.externalCSS, n.write("<html><head>" + (t ? '<link href="' + t + '" rel="stylesheet" type="text/css" />' : "") + '<\/head><body id="nicEditContent" style="margin: 0 !important; background-color: transparent !important;">' + this.initialContent + "<\/body><\/html>"), n.close(), this.frameDoc = n, this.frameWin = $BK(this.elmFrame.contentWindow), this.frameContent = $BK(this.frameWin.document.body).setStyle(this.savedStyles), this.instanceDoc = this.frameWin.document.defaultView, this.heightUpdate(), this.frameDoc.addEvent("mousedown", this.selected.closureListener(this)).addEvent("keyup", this.heightUpdate.closureListener(this)).addEvent("keydown", this.keyDown.closureListener(this)).addEvent("keyup", this.selected.closure(this)), this.ne.fireEvent("add", this)
    }, getElm: function () {
        return this.frameContent
    }, setContent: function (n) {
        this.content = n, this.ne.fireEvent("set", this), this.frameContent.innerHTML = this.content, this.heightUpdate()
    }, getSel: function () {
        return this.frameWin ? this.frameWin.getSelection() : this.frameDoc.selection
    }, heightUpdate: function () {
        this.elmFrame.style.height = Math.max(this.frameContent.offsetHeight, this.initialHeight) + "px"
    }, nicCommand: function (n, t) {
        this.frameDoc.execCommand(n, !1, t), setTimeout(this.heightUpdate.closure(this), 100)
    }
}), nicEditorPanel = bkClass.extend({
    construct: function (n, t, i) {
        this.elm = n, this.options = t, this.ne = i, this.panelButtons = [], this.buttonList = bkExtend([], this.ne.options.buttonList), this.panelContain = new bkElement("DIV").setStyle({ overflow: "hidden", width: "100%", border: "1px solid #cccccc", backgroundColor: "#efefef" }).addClass("panelContain"), this.panelElm = new bkElement("DIV").setStyle({ margin: "2px", marginTop: "0px", zoom: 1, overflow: "hidden" }).addClass("panel").appendTo(this.panelContain), this.panelContain.appendTo(n);
        var r = this.ne.options, u = r.buttons;
        for (button in u)
            this.addButton(button, r, !0);
        this.reorder(), n.noSelect()
    }, addButton: function (buttonName, options, noOrder) {
        var button = options.buttons[buttonName], type = button.type ? eval("(typeof(" + button.type + ') == "undefined") ? null : ' + button.type + ";") : nicEditorButton, hasButton = bkLib.inArray(this.buttonList, buttonName);
        type && (hasButton || this.ne.options.fullPanel) && (this.panelButtons.push(new type(this.panelElm, buttonName, options, this.ne)), hasButton || this.buttonList.push(buttonName))
    }, findButton: function (n) {
        for (var t = 0; t < this.panelButtons.length; t++)
            if (this.panelButtons[t].name == n)
                return this.panelButtons[t]
    }, reorder: function () {
        for (var i = this.buttonList, t, n = 0; n < i.length; n++)
            t = this.findButton(i[n]), t && this.panelElm.appendChild(t.margin)
    }, remove: function () {
        this.elm.remove()
    }
}), nicEditorButton = bkClass.extend({
    construct: function (n, t, i, r) {
        this.options = i.buttons[t], this.name = t, this.ne = r, this.elm = n, this.margin = new bkElement("DIV").setStyle({ float: "left", marginTop: "2px" }).appendTo(n), this.contain = new bkElement("DIV").setStyle({ width: "20px", height: "20px" }).addClass("buttonContain").appendTo(this.margin), this.border = new bkElement("DIV").setStyle({ backgroundColor: "#efefef", border: "1px solid #efefef" }).appendTo(this.contain), this.button = new bkElement("DIV").setStyle({ width: "18px", height: "18px", overflow: "hidden", zoom: 1, cursor: "pointer" }).addClass("button").setStyle(this.ne.getIcon(t, i)).appendTo(this.border), this.button.addEvent("mouseover", this.hoverOn.closure(this)).addEvent("mouseout", this.hoverOff.closure(this)).addEvent("mousedown", this.mouseClick.closure(this)).noSelect(), window.opera || (this.button.onmousedown = this.button.onclick = bkLib.cancelEvent), r.addEvent("selected", this.enable.closure(this)).addEvent("blur", this.disable.closure(this)).addEvent("key", this.key.closure(this)), this.disable(), this.init()
    }, init: function () {
    }, hide: function () {
        this.contain.setStyle({ display: "none" })
    }, updateState: function () {
        this.isDisabled ? this.setBg() : this.isHover ? this.setBg("hover") : this.isActive ? this.setBg("active") : this.setBg()
    }, setBg: function (n) {
        var t;
        switch (n) {
            case "hover":
                t = { border: "1px solid #666", backgroundColor: "#ddd" };
                break;
            case "active":
                t = { border: "1px solid #666", backgroundColor: "#ccc" };
                break;
            default:
                t = { border: "1px solid #efefef", backgroundColor: "#efefef" }
        }
        this.border.setStyle(t).addClass("button-" + n)
    }, checkNodes: function (n) {
        var t = n;
        do
            if (this.options.tags && bkLib.inArray(this.options.tags, t.nodeName))
                return this.activate(), !0;
        while (t = t.parentNode && t.className != "nicEdit");
        for (t = $BK(n) ; t.nodeType == 3;)
            t = $BK(t.parentNode);
        if (this.options.css)
            for (itm in this.options.css)
                if (t.getStyle(itm, this.ne.selectedInstance.instanceDoc) == this.options.css[itm])
                    return this.activate(), !0;
        return this.deactivate(), !1
    }, activate: function () {
        this.isDisabled || (this.isActive = !0, this.updateState(), this.ne.fireEvent("buttonActivate", this))
    }, deactivate: function () {
        this.isActive = !1, this.updateState(), this.isDisabled || this.ne.fireEvent("buttonDeactivate", this)
    }, enable: function (n, t) {
        this.isDisabled = !1, this.contain.setStyle({ opacity: 1 }).addClass("buttonEnabled"), this.updateState(), this.checkNodes(t)
    }, disable: function () {
        this.isDisabled = !0, this.contain.setStyle({ opacity: .6 }).removeClass("buttonEnabled"), this.updateState()
    }, toggleActive: function () {
        this.isActive ? this.deactivate() : this.activate()
    }, hoverOn: function () {
        this.isDisabled || (this.isHover = !0, this.updateState(), this.ne.fireEvent("buttonOver", this))
    }, hoverOff: function () {
        this.isHover = !1, this.updateState(), this.ne.fireEvent("buttonOut", this)
    }, mouseClick: function () {
        this.options.command && (this.ne.nicCommand(this.options.command, this.options.commandArgs), this.options.noActive || this.toggleActive()), this.ne.fireEvent("buttonClick", this)
    }, key: function (n, t) {
        this.options.key && t.ctrlKey && String.fromCharCode(t.keyCode || t.charCode).toLowerCase() == this.options.key && (this.mouseClick(), t.preventDefault && t.preventDefault())
    }
}), nicPlugin = bkClass.extend({
    construct: function (n, t) {
        this.options = t, this.ne = n, this.ne.addEvent("panel", this.loadPanel.closure(this)), this.init()
    }, loadPanel: function (n) {
        var i = this.options.buttons, t;
        for (t in i)
            n.addButton(t, this.options);
        n.reorder()
    }, init: function () {
    }
})


var nicPaneOptions = {};

var nicEditorPane = bkClass.extend({
    construct: function (D, C, B, A) {
        this.ne = C;
        this.elm = D;
        this.pos = D.pos();
        this.contain = new bkElement("div").setStyle({ zIndex: "99999", overflow: "hidden", position: "absolute", left: this.pos[0] + "px", top: this.pos[1] + "px" });
        this.pane = new bkElement("div").setStyle({ fontSize: "12px", border: "1px solid #ccc", overflow: "hidden", padding: "4px", textAlign: "left", backgroundColor: "#ffffc9" }).addClass("pane").setStyle(B).appendTo(this.contain);
        if (A && !A.options.noClose) {
            this.close = new bkElement("div").setStyle({ "float": "right", height: "16px", width: "16px", cursor: "pointer" }).setStyle(this.ne.getIcon("close", nicPaneOptions)).addEvent("mousedown", A.removePane.closure(this)).appendTo(this.pane)
        }
        this.contain.noSelect().appendTo(document.body);
        this.position();
        this.init()
    }, init: function () {
    }, position: function () {
        if (this.ne.nicPanel) {
            var B = this.ne.nicPanel.elm;
            var A = B.pos();
            var C = A[0] + parseInt(B.getStyle("width")) - (parseInt(this.pane.getStyle("width")) + 8);
            if (C < this.pos[0]) {
                this.contain.setStyle({ left: C + "px" })
            }
        }
    }, toggle: function () {
        this.isVisible = !this.isVisible;
        this.contain.setStyle({ display: ((this.isVisible) ? "block" : "none") })
    }, remove: function () {
        if (this.contain) {
            this.contain.remove();
            this.contain = null
        }
    }, append: function (A) {
        A.appendTo(this.pane)
    }, setContent: function (A) {
        this.pane.setContent(A)
    }
});

var nicEditorAdvancedButton = nicEditorButton.extend({
    init: function () {
        this.ne.addEvent("selected", this.removePane.closure(this)).addEvent("blur", this.removePane.closure(this))
    }, mouseClick: function () {
        if (!this.isDisabled) {
            if (this.pane && this.pane.pane) {
                this.removePane()
            } else {
                this.pane = new nicEditorPane(this.contain, this.ne, { width: (this.width || "270px"), backgroundColor: "#fff" }, this);
                this.addPane();
                this.ne.selectedInstance.saveRng()
            }
        }
    }, addForm: function (C, G) {
        this.form = new bkElement("form").addEvent("submit", this.submit.closureListener(this));
        this.pane.append(this.form);
        this.inputs = {};
        for (itm in C) {
            var D = C[itm];
            var F = "";
            if (G) {
                F = G.getAttribute(itm)
            }
            if (!F) {
                F = D.value || ""
            }
            var A = C[itm].type;
            if (A == "title") {
                new bkElement("div").setContent(D.txt).setStyle({ fontSize: "14px", fontWeight: "bold", padding: "0px", margin: "2px 0" }).appendTo(this.form)
            } else {
                var B = new bkElement("div").setStyle({ overflow: "hidden", clear: "both" }).appendTo(this.form);
                if (D.txt) {
                    new bkElement("label").setAttributes({ "for": itm }).setContent(D.txt).setStyle({ margin: "2px 4px", fontSize: "13px", width: "50px", lineHeight: "20px", textAlign: "right", "float": "left" }).appendTo(B)
                }
                switch (A) {
                    case "text":
                        this.inputs[itm] = new bkElement("input").setAttributes({ id: itm, value: F, type: "text" }).setStyle({ margin: "2px 0", fontSize: "13px", "float": "left", height: "20px", border: "1px solid #ccc", overflow: "hidden" }).setStyle(D.style).appendTo(B);
                        break;
                    case "select":
                        this.inputs[itm] = new bkElement("select").setAttributes({ id: itm }).setStyle({ border: "1px solid #ccc", "float": "left", margin: "2px 0" }).appendTo(B);
                        for (opt in D.options) {
                            var E = new bkElement("option").setAttributes({ value: opt, selected: (opt == F) ? "selected" : "" }).setContent(D.options[opt]).appendTo(this.inputs[itm])
                        }
                        break;
                    case "content":
                        this.inputs[itm] = new bkElement("textarea").setAttributes({ id: itm }).setStyle({ border: "1px solid #ccc", "float": "left" }).setStyle(D.style).appendTo(B);
                        this.inputs[itm].value = F
                }
            }
        }
        new bkElement("input").setAttributes({ type: "submit" }).setStyle({ backgroundColor: "#efefef", border: "1px solid #ccc", margin: "3px 0", "float": "left", clear: "both" }).appendTo(this.form);
        this.form.onsubmit = bkLib.cancelEvent
    }, submit: function () {
    }, findElm: function (B, A, E) {
        var D = this.ne.selectedInstance.getElm().getElementsByTagName(B);
        for (var C = 0; C < D.length; C++) {
            if (D[C].getAttribute(A) == E) {
                return $BK(D[C])
            }
        }
    }, removePane: function () {
        if (this.pane) {
            this.pane.remove();
            this.pane = null;
            this.ne.selectedInstance.restoreRng()
        }
    }
});

var nicButtonTips = bkClass.extend({
    construct: function (A) {
        this.ne = A;
        A.addEvent("buttonOver", this.show.closure(this)).addEvent("buttonOut", this.hide.closure(this))
    }, show: function (A) {
        this.timer = setTimeout(this.create.closure(this, A), 400)
    }, create: function (A) {
        this.timer = null;
        if (!this.pane) {
            this.pane = new nicEditorPane(A.button, this.ne, { fontSize: "12px", marginTop: "5px" });
            this.pane.setContent(A.options.name)
        }
    }, hide: function (A) {
        if (this.timer) {
            clearTimeout(this.timer)
        }
        if (this.pane) {
            this.pane = this.pane.remove()
        }
    }
});
nicEditors.registerPlugin(nicButtonTips);


var nicSelectOptions = {
    buttons: {
        'fontSize': { name: __('Select Font Size'), type: 'nicEditorFontSizeSelect', command: 'fontsize' },
        'fontFamily': { name: __('Select Font Family'), type: 'nicEditorFontFamilySelect', command: 'fontname' },
        'fontFormat': { name: __('Select Font Format'), type: 'nicEditorFontFormatSelect', command: 'formatBlock' }
    }
};

var nicEditorSelect = bkClass.extend({
    construct: function (D, A, C, B) {
        this.options = C.buttons[A];
        this.elm = D;
        this.ne = B;
        this.name = A;
        this.selOptions = new Array();
        this.margin = new bkElement("div").setStyle({ "float": "left", margin: "2px 1px 0 1px" }).appendTo(this.elm);
        this.contain = new bkElement("div").setStyle({ width: "100px", height: "20px", cursor: "pointer", overflow: "hidden" }).addClass("selectContain").addEvent("click", this.toggle.closure(this)).appendTo(this.margin);
        this.items = new bkElement("div").setStyle({ overflow: "hidden", zoom: 1, border: "1px solid #ccc", paddingLeft: "3px", backgroundColor: "#fff" }).appendTo(this.contain);
        this.control = new bkElement("div").setStyle({ overflow: "hidden", "float": "right", height: "18px", width: "16px" }).addClass("selectControl").setStyle(this.ne.getIcon("arrow", C)).appendTo(this.items);
        this.txt = new bkElement("div").setStyle({ overflow: "hidden", "float": "left", width: "66px", height: "14px", marginTop: "1px", fontFamily: "sans-serif", textAlign: "center", fontSize: "12px" }).addClass("selectTxt").appendTo(this.items);
        if (!window.opera) {
            this.contain.onmousedown = this.control.onmousedown = this.txt.onmousedown = bkLib.cancelEvent
        }
        this.margin.noSelect();
        this.ne.addEvent("selected", this.enable.closure(this)).addEvent("blur", this.disable.closure(this));
        this.disable();
        this.init()
    }, disable: function () {
        this.isDisabled = true;
        this.close();
        this.contain.setStyle({ opacity: 0.6 })
    }, enable: function (A) {
        this.isDisabled = false;
        this.close();
        this.contain.setStyle({ opacity: 1 })
    }, setDisplay: function (A) {
        this.txt.setContent(A)
    }, toggle: function () {
        if (!this.isDisabled) {
            (this.pane) ? this.close() : this.open()
        }
    }, open: function () {
        this.pane = new nicEditorPane(this.items, this.ne, { width: "98px", padding: "0px", borderTop: 0, borderLeft: "1px solid #ccc", borderRight: "1px solid #ccc", borderBottom: "0px", backgroundColor: "#fff" });
        for (var C = 0; C < this.selOptions.length; C++) {
            var B = this.selOptions[C];
            var A = new bkElement("div").setStyle({ overflow: "hidden", borderBottom: "1px solid #ccc", width: "98px", textAlign: "left", overflow: "hidden", cursor: "pointer" });
            var D = new bkElement("div").setStyle({ padding: "0px 4px" }).setContent(B[1]).appendTo(A).noSelect();
            D.addEvent("click", this.update.closure(this, B[0])).addEvent("mouseover", this.over.closure(this, D)).addEvent("mouseout", this.out.closure(this, D)).setAttributes("id", B[0]);
            this.pane.append(A);
            if (!window.opera) {
                D.onmousedown = bkLib.cancelEvent
            }
        }
    }, close: function () {
        if (this.pane) {
            this.pane = this.pane.remove()
        }
    }, over: function (A) {
        A.setStyle({ backgroundColor: "#ccc" })
    }, out: function (A) {
        A.setStyle({ backgroundColor: "#fff" })
    }, add: function (B, A) {
        this.selOptions.push(new Array(B, A))
    }, update: function (A) {
        this.ne.nicCommand(this.options.command, A);
        this.close()
    }
});
var nicEditorFontSizeSelect = nicEditorSelect.extend({
    sel: { 1: "1&nbsp;(8pt)", 2: "2&nbsp;(10pt)", 3: "3&nbsp;(12pt)", 4: "4&nbsp;(14pt)", 5: "5&nbsp;(18pt)", 6: "6&nbsp;(24pt)" }, init: function () {
        this.setDisplay("Font&nbsp;Size...");
        for (itm in this.sel) {
            this.add(itm, '<font size="' + itm + '">' + this.sel[itm] + "</font>")
        }
    }
});
var nicEditorFontFamilySelect = nicEditorSelect.extend({
    sel: { arial: "Arial", "comic sans ms": "Comic Sans", "courier new": "Courier New", georgia: "Georgia", helvetica: "Helvetica", impact: "Impact", "times new roman": "Times", "trebuchet ms": "Trebuchet", verdana: "Verdana" }, init: function () {
        this.setDisplay("Font&nbsp;Family...");
        for (itm in this.sel) {
            this.add(itm, '<font face="' + itm + '">' + this.sel[itm] + "</font>")
        }
    }
});
var nicEditorFontFormatSelect = nicEditorSelect.extend({
    sel: { p: "Paragraph", pre: "Pre", h6: "Heading&nbsp;6", h5: "Heading&nbsp;5", h4: "Heading&nbsp;4", h3: "Heading&nbsp;3", h2: "Heading&nbsp;2", h1: "Heading&nbsp;1" }, init: function () {
        this.setDisplay("Font&nbsp;Format...");
        for (itm in this.sel) {
            var A = itm.toUpperCase();
            this.add("<" + A + ">", "<" + itm + ' style="padding: 0px; margin: 0px;">' + this.sel[itm] + "</" + A + ">")
        }
    }
});
nicEditors.registerPlugin(nicPlugin, nicSelectOptions);


var nicLinkOptions = {
    buttons: {
        'link': { name: 'Add Link', type: 'nicLinkButton', tags: ['A'] },
        'unlink': { name: 'Remove Link', command: 'unlink', noActive: true }
    }
};

var nicLinkButton = nicEditorAdvancedButton.extend({
    addPane: function () {
        this.ln = this.ne.selectedInstance.selElm().parentTag("A");
        this.addForm({ "": { type: "title", txt: "Add/Edit Link" }, href: { type: "text", txt: "URL", value: "http://", style: { width: "150px" } }, title: { type: "text", txt: "Title" }, target: { type: "select", txt: "Open In", options: { "": "Current Window", _blank: "New Window" }, style: { width: "100px" } } }, this.ln)
    }, submit: function (C) {
        var A = this.inputs.href.value;
        if (A == "http://" || A == "") {
            alert("You must enter a URL to Create a Link");
            return false
        }
        this.removePane();
        if (!this.ln) {
            var B = "javascript:nicTemp();";
            this.ne.nicCommand("createlink", B);
            this.ln = this.findElm("A", "href", B)
        }
        if (this.ln) {
            this.ln.setAttributes({ href: this.inputs.href.value, title: this.inputs.title.value, target: this.inputs.target.options[this.inputs.target.selectedIndex].value })
        }
    }
});
nicEditors.registerPlugin(nicPlugin, nicLinkOptions);


var nicColorOptions = {
    buttons: {
        'forecolor': { name: __('Change Text Color'), type: 'nicEditorColorButton', noClose: true },
        'bgcolor': { name: __('Change Background Color'), type: 'nicEditorBgColorButton', noClose: true }
    }
};

var nicEditorColorButton = nicEditorAdvancedButton.extend({
    addPane: function () {
        var D = { 0: "00", 1: "33", 2: "66", 3: "99", 4: "CC", 5: "FF" };
        var H = new bkElement("DIV").setStyle({ width: "270px" });
        for (var A in D) {
            for (var F in D) {
                for (var E in D) {
                    var I = "#" + D[A] + D[E] + D[F];
                    var C = new bkElement("DIV").setStyle({ cursor: "pointer", height: "15px", "float": "left" }).appendTo(H);
                    var G = new bkElement("DIV").setStyle({ border: "2px solid " + I }).appendTo(C);
                    var B = new bkElement("DIV").setStyle({ backgroundColor: I, overflow: "hidden", width: "11px", height: "11px" }).addEvent("click", this.colorSelect.closure(this, I)).addEvent("mouseover", this.on.closure(this, G)).addEvent("mouseout", this.off.closure(this, G, I)).appendTo(G);
                    if (!window.opera) {
                        C.onmousedown = B.onmousedown = bkLib.cancelEvent
                    }
                }
            }
        }
        this.pane.append(H.noSelect())
    }, colorSelect: function (A) {
        this.ne.nicCommand("foreColor", A);
        this.removePane()
    }, on: function (A) {
        A.setStyle({ border: "2px solid #000" })
    }, off: function (A, B) {
        A.setStyle({ border: "2px solid " + B })
    }
});
var nicEditorBgColorButton = nicEditorColorButton.extend({
    colorSelect: function (A) {
        this.ne.nicCommand("hiliteColor", A);
        this.removePane()
    }
});
nicEditors.registerPlugin(nicPlugin, nicColorOptions);


var nicImageOptions = {
	buttons : {
		'image' : {name : 'Add Image', type : 'nicImageButton', tags : ['IMG']}
	}
	
};

var nicImageButton = nicEditorAdvancedButton.extend({
    addPane: function () {
        this.im = this.ne.selectedInstance.selElm().parentTag("IMG");
        this.addForm({
            "": { type: "title", txt: "Add/Edit Image" }, src: { type: "text", txt: "URL", value: "http://", style: { width: "150px" } },
            alt: { type: "text", txt: "Alt Text", style: { width: "100px" } }, align: {
                type: "select", txt: "Align",
                options: { none: "Default", left: "Left", right: "Right" }
            }
        }, this.im)
    }, submit: function (B) {
        var C = this.inputs.src.value;
        if (C == "" || C == "http://") {
            alert("You must enter a Image URL to insert"); return false
        }
        this.removePane();
        if (!this.im) {
            var A = "javascript:nicImTemp();";
            this.ne.nicCommand("insertImage", A);
            this.im = this.findElm("IMG", "src", A)
        }
        if (this.im) {
            this.im.setAttributes({
                src: this.inputs.src.value,
                alt: this.inputs.alt.value,
                align: this.inputs.align.value
            })
        }
    }
}); nicEditors.registerPlugin(nicPlugin, nicImageOptions);


var nicCodeOptions = {
    buttons: {
        'xhtml': { name: 'Edit HTML', type: 'nicCodeButton' }
    }

};
var nicCodeButton = nicEditorAdvancedButton.extend({ width: "350px", addPane: function () { this.addForm({ "": { type: "title", txt: "Edit HTML" }, code: { type: "content", value: this.ne.selectedInstance.getContent(), style: { width: "340px", height: "200px" } } }) }, submit: function (B) { var A = this.inputs.code.value; this.ne.selectedInstance.setContent(A); this.removePane() } }); nicEditors.registerPlugin(nicPlugin, nicCodeOptions);



var nicUploadOptions = {
	buttons : {
	    'upload': { name: 'Upload Image', type: 'nicUploadButton', imageNames: new Array() }
	}
};

var nicUploadButton = nicEditorAdvancedButton.extend({
    nicURI: "/FAQ/Upload",//"http://api.imgur.com/2/upload.json",//
    errorText: "Failed to upload image",
    addPane: function () {
        if (typeof window.FormData === "undefined" ||
            (navigator.userAgent != null && navigator.userAgent.indexOf("MSIE") != -1 )) {
            return this.onError("Image uploads are not supported in this browser, use Chrome, Firefox, or Safari instead.")
        }
        this.im = this.ne.selectedInstance.selElm().parentTag("IMG");
        var A = new bkElement("div").setStyle({ padding: "10px" }).appendTo(this.pane.pane);
        new bkElement("div").setStyle({ fontSize: "14px", fontWeight: "bold", paddingBottom: "5px" }).setContent("Insert an Image").appendTo(A);
        new bkElement("label").setAttributes({ id: "widthLbl"}).setContent("Image width: ").appendTo(A);
        this.fileInputWidth = new bkElement("input").setAttributes({ type: "text", id: "widthText", value: "400" }).appendTo(A);
        this.fileInput = new bkElement("input").setAttributes({ type: "file", id: "uploadBtn" }).appendTo(A);
        this.progress = new bkElement("progress").setStyle({ width: "100%", display: "none" }).setAttributes("max", 100).appendTo(A);
        this.fileInput.onchange = this.uploadFile.closure(this)
    }, onError: function (A) { this.removePane(); alert(A || "Failed to upload image") },
    uploadFile: function () {
        var B = this.fileInput.files[0];
        if (!B || !B.type.match(/image.*/)) {
            this.onError("Only image files can be uploaded");
            return;
        }
        if (B.size > 2097152) {
            this.onError("Images are less than 2 MB can be uploaded");
            return;
        }
        this.fileInput.setStyle({ display: "none" });
        this.fileInputWidth.setStyle({ display: "none" });
        this.setProgress(0);
        var A = new FormData();
        A.append("image", B);
        A.append("key", "b7ea18a4ecbda8e92203fa4968d10660");
        var C = new XMLHttpRequest();
        C.open("POST", this.ne.options.uploadURI || this.nicURI);
        C.onload = function () {
            try {
                var D = JSON.parse(C.responseText)
            }
            catch (E) {
                return this.onError()
            }
            this.onUploaded(D.upload)
        }.closure(this);
        C.onerror = this.onError.closure(this);
        C.upload.onprogress = function (D) {
            this.setProgress(D.loaded / D.total)
        }.closure(this);
        C.send(A)
    },
    setProgress: function (A) {
        this.progress.setStyle({ display: "block" });
        if (A < 0.98) {
            this.progress.value = A
        } else {
            this.progress.removeAttribute("value")
        }
    },
    onUploaded: function (B) {
        this.removePane();
        var D = B.links.original;
        if (!this.im) {
            this.ne.selectedInstance.restoreRng();
            var C = "javascript:nicImTemp();";
            this.ne.nicCommand("insertImage", D);
            this.im = this.findElm("IMG", "src", D)
        }
        var A = parseInt(this.ne.selectedInstance.elm.getStyle("width"));
        var userImageWidth = parseInt(this.fileInputWidth.value);
        if (A && userImageWidth) {
            A = Math.min(A, userImageWidth);
        }
        if (this.im) {
            this.im.setAttributes({
                src: D,
                width: (A && B.image.width) ? Math.min(A, B.image.width) : ""
            })

            this.options.imageNames.push(B.image.name);
        }
    }
}); nicEditors.registerPlugin(nicPlugin, nicUploadOptions);


