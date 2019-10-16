"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Project = /** @class */ (function () {
    function Project() {
    }
    return Project;
}());
exports.Project = Project;
var WorkflowElementCategory = /** @class */ (function () {
    function WorkflowElementCategory() {
    }
    return WorkflowElementCategory;
}());
exports.WorkflowElementCategory = WorkflowElementCategory;
var WorkflowElement = /** @class */ (function () {
    function WorkflowElement() {
    }
    return WorkflowElement;
}());
exports.WorkflowElement = WorkflowElement;
var ASVSLevel;
(function (ASVSLevel) {
    ASVSLevel[ASVSLevel["LevelOne"] = 1] = "LevelOne";
    ASVSLevel[ASVSLevel["LevelTwo"] = 2] = "LevelTwo";
    ASVSLevel[ASVSLevel["LevelThree"] = 3] = "LevelThree";
})(ASVSLevel = exports.ASVSLevel || (exports.ASVSLevel = {}));
var EnumEx = /** @class */ (function () {
    function EnumEx() {
    }
    EnumEx.getNamesAndValues = function (e) {
        return EnumEx.getNames(e).map(function (n) { return ({ name: n, value: e[n] }); });
    };
    EnumEx.getNames = function (e) {
        return Object.keys(e).filter(function (k) { return typeof e[k] === "number"; });
    };
    return EnumEx;
}());
exports.EnumEx = EnumEx;
//# sourceMappingURL=project.js.map