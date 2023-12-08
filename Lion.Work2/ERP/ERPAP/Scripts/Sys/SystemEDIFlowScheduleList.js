gantt.config.columns = [
    { name: "EDIFlowNM", label: "EDIFlowNM", tree: true, width: 450, resize: true },
    { name: "SCHFrequency", label: "SCHFrequency", align: "center", width: 150, resize: true },
    { name: "Remark", label: "Remark", align: "center", width: 300, resize: true }
];

gantt.config.date_format = "%H:%i";
gantt.config.min_column_width = 20;
gantt.config.duration_unit = "minute";
gantt.config.duration_step = 60;
gantt.config.scale_height = 75;

gantt.config.scales = [
    { unit: "hour", step: 1, format: "%H:%i" },
    { unit: "minute", step: 10, format: "%i" }
];

gantt.templates.task_text = function (start, end, task) {
    return "";
};

gantt.config.layout = {
    css: "gantt_container",
    cols: [
        {
            width: 400,
            min_width: 300,
            rows: [
                { view: "grid", scrollX: "gridScroll", scrollable: true, scrollY: "scrollVer" },
                { view: "scrollbar", id: "gridScroll", group: "horizontal" }
            ]
        },
        { resizer: true, width: 1 },
        {
            rows: [
                { view: "timeline", scrollX: "scrollHor", scrollY: "scrollVer" },
                { view: "scrollbar", id: "scrollHor", group: "horizontal" }
            ]
        },
        { view: "scrollbar", id: "scrollVer" }
    ]
};

gantt.init("gantt_here");
gantt.parse(schedule_data);
