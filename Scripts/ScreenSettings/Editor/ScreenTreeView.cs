using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;
using System.Linq;
using Common.Entity;

public class ScreenTreeView : TreeView
{
    ScreenSettings screenSettings;

    public ScreenTreeView(TreeViewState state, ScreenSettings screenSettings) : base(state)
    {
        this.screenSettings = screenSettings;
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

        for (int i = 0; i < screenSettings.windows.Count(); i++)
        {
            var window = screenSettings.windows[i];
            var windowItem = new TreeViewItem(window.id, -1, window.name);
            root.AddChild(windowItem);
            rows.Add(windowItem);

            var screens = screenSettings.screens.FindAll(x => x.windowId == window.id);
            foreach (var screen in screens)
            {
                var screenItem = new TreeViewItem(screen.id, -1, screen.name);
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
        var item = this.GetRows().First(x => x.id == args.itemID);
        item.displayName = args.newName;
        base.RenameEnded(args);
        TreeToSettings();
        Reload();
    }



    protected override void KeyEvent()
    {
        base.KeyEvent();
        var e = Event.current;
        if (e.type == EventType.KeyDown)
        {
            if (e.keyCode == KeyCode.A)
            {
                OnKeyDownA();
                e.Use();
            }

            if (e.keyCode == KeyCode.S)
            {
                OnKeyDownS();
                e.Use();
            }

            if (e.keyCode == KeyCode.D)
            {
                OnKeyDownDelete();
                e.Use();
            }

            if (e.keyCode == KeyCode.U)
            {
                OnKeyDownU();
                e.Use();
            }

            if (e.keyCode == KeyCode.I)
            {
                OnKeyDownI();
                e.Use();
            }
        }
    }

    // 子を追加
    void OnKeyDownA()
    {
        var selections = GetSelection();
        if (selections.Count() != 1)
            return;
        int id = selections.First();
        var item = this.GetRows().First(x => x.id == id);
        if (item.parent != rootItem)
            return;
        var newItem = new TreeViewItem(-1, -1, "Screen");
        item.AddChild(newItem);
        SetupIdsFromParentsAndChildren();
        // フォーカス
        this.SetSelection(new List<int> { newItem.id });
        this.SetExpanded(id, true);
        TreeToSettings();
        Reload();
    }

    // 兄弟を追加
    void OnKeyDownS()
    {
        var selections = GetSelection();
        if (selections.Count() != 1)
            return;
        var item = this.GetRows().First(x => x.id == selections.First());

        int index = item.parent.children.IndexOf(item) + 1;
        bool isWindow = (item.id / 100 == 0);
        string name = isWindow ? "Window" : "Screen";
        var newItem = new TreeViewItem(-1, -1, name);
        item.parent.InsertChild(index, newItem);
        SetupIdsFromParentsAndChildren();
        // フォーカス
        this.SetSelection(new List<int> { newItem.id });
        TreeToSettings();
        Reload();
    }

    void OnKeyDownDelete()
    {
        var selections = GetSelection();
        if (selections.Count() != 1)
            return;
        var rows = GetRows();
        var item = rows.First(x => x.id == selections.First());
        int index = rows.IndexOf(item);
        item.parent.children.Remove(item);
        SetupIdsFromParentsAndChildren();
        if (index != 0)
            this.SetSelection(new List<int> { rows[index - 1].id });
        TreeToSettings();
        Reload();
    }

    void TreeToSettings()
    {
        var windowItems = rootItem.children;
        List<WindowEntity> windows = new List<WindowEntity>();
        List<ScreenEntity> screens = new List<ScreenEntity>();
        foreach (var windowItem in windowItems)
        {
            windows.Add(new WindowEntity(windowItem.id, windowItem.displayName));
            if (!windowItem.hasChildren)
                continue;
            screens.AddRange(
                windowItem.children
                .Select(x => new ScreenEntity(x.id, windowItem.id, x.displayName))
            );
        }
        screenSettings.windows = windows;
        screenSettings.screens = screens;
        EditorUtility.SetDirty(screenSettings);
    }

    // indexをidにする．
    void SetupIdsFromParentsAndChildren()
    {
        var windowItems = rootItem.children;
        for (int i = 0; i < windowItems.Count(); i++)
        {
            windowItems[i].id = i + 1;
            if (!windowItems[i].hasChildren)
                continue;
            var screenItems = windowItems[i].children;
            for (int j = 0; j < screenItems.Count(); j++)
            {
                screenItems[j].id = windowItems[i].id * 100 + j + 1;
            }
        }
    }

    void OnKeyDownU()
    {
        var selections = GetSelection();
        if (selections.Count() != 1)
            return;
        var rows = GetRows();
        var item = rows.First(x => x.id == selections.First());
        MoveUp(item);
    }

    void OnKeyDownI()
    {
        var selections = GetSelection();
        if (selections.Count() != 1)
            return;
        var rows = GetRows();
        var item = rows.First(x => x.id == selections.First());
        MoveDown(item);
    }

    void MoveUp(TreeViewItem item)
    {
        var children = item.parent.children;
        var index = children.IndexOf(item);
        if (index == 0)
            return;
        Swap(children, index, index - 1);
        SetupIdsFromParentsAndChildren();
        this.SetSelection(new List<int> { item.id });
        TreeToSettings();
        Reload();
    }

    void MoveDown(TreeViewItem item)
    {
        var children = item.parent.children;
        var index = children.IndexOf(item);
        if (index == children.Count() - 1)
            return;
        Swap(children, index, index + 1);
        SetupIdsFromParentsAndChildren();
        this.SetSelection(new List<int> { item.id });
        TreeToSettings();
        Reload();
    }

    public static void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }
}

public static class TreeViewItemExt
{
    public static void InsertChild(this TreeViewItem parent, int index, TreeViewItem child)
    {
        parent.children.Insert(index, child);
        child.parent = parent;
    }
}
