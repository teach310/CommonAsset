using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;
using System.Linq;
using Common.Entity;

public class ScreenTreeView : TreeView
{

    List<ScreenSettingsTreeModel> dataList = new List<ScreenSettingsTreeModel>();

    public ScreenTreeView(TreeViewState state, List<ScreenSettingsTreeModel> dataList) : base(state)
    {
        this.dataList = dataList;
    }

    protected override TreeViewItem BuildRoot()
    {
        var root = new TreeViewItem(id: 1, depth: -1, displayName: "ルート");
        return root;
    }

    protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
    {
        if (root.children != null)
            root.children.Clear();
        List<TreeViewItem> rows = new List<TreeViewItem>();
        var windows = dataList.Where(x => !x.IsScreen()).ToList();
        for (int i = 0; i < windows.Count(); i++)
        {
            var window = windows[i];
            var windowItem = new TreeViewItem(window.id, -1, window.displayName);
            root.AddChild(windowItem);
            rows.Add(windowItem);

            var screens = dataList
                .Where(x => x.IsScreen())
                .Where(x => x.parent.id == window.id);
            foreach (var item in screens)
            {
                var screenItem = new TreeViewItem(item.id, -1, item.displayName);
                windowItem.AddChild(screenItem);
                rows.Add(screenItem);
            }
        }
        SetupDepthsFromParentsAndChildren(root);
        return base.BuildRows(root);
    }

    protected override bool CanRename(TreeViewItem item)
    {
        return true;
    }

    protected override void RenameEnded(TreeView.RenameEndedArgs args)
    {
        var item = dataList.Find(x => x.id == args.itemID);
        item.displayName = args.newName;
        ScreenSettingsUseCase.SaveName(item);

        base.RenameEnded(args);
        Reload();
    }



    protected override void KeyEvent()
    {
        base.KeyEvent();
        var e = Event.current;
        if (e.type == EventType.KeyDown)
        {
            if (e.keyCode == KeyCode.Tab)
            {
                OnKeyDownTab();
            }
        }
    }

    void OnKeyDownTab()
    {
        var selections = GetSelection();
        if (selections.Count() != 1)
            return;
        var id = selections.First();
        var model = dataList.Find(x => x.id == id);
        var item = this.GetRows().First(x => x.id == selections.First());

        if (model.IsScreen())
        {
            AddScreen();
            dataList.Insert(dataList.IndexOf(model) + 1
                            , new ScreenSettingsTreeModel(id, item.children.Count(), parent: model));
        }else{
            AddWindow();
        }
        // フォーカス
        ScreenSettingsUseCase.Save(dataList);
        Reload();
        Debug.Log(selections[0]);
    }

    // 並行にWindowを増やす
    void AddWindow() { 
    }

	
}
