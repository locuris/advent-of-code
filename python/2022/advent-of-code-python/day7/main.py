from anytree import AnyNode, RenderTree
# This is a sample Python script.

# Press ⌃R to execute it or replace it with your code.
# Press Double ⇧ to search everywhere for classes, files, tool windows, actions, and settings.


def print_hi():
    # Use a breakpoint in the code line below to debug your script.
    f = open("day_7.txt", "r")
    lines = f.readlines()
    tree = AnyNode(id="", size=0, depth=0)
    cd = AnyNode(id="/", size=0, depth=0, parent=tree)
    for line in lines:
        depth = 0
        line = line.replace("\n", "")
        #if line.endswith("/"):
         #   continue
        if line.startswith("$ cd"):
            if line.startswith("$ cd .."):
                cd = cd.parent
                depth -= 1
            else:
                depth += 1
                d = line.replace("$ cd ", "")
                cd = AnyNode(id=d, size=0, depth=depth, parent=cd)
        elif line.startswith("dir") or line.startswith("$"):
            continue
        else:
            size = int(line.split(" ")[0])
            cd.size += size
            parent = cd.parent
            while parent is not None:
                parent.size += size
                parent = parent.parent

    answer = 0
    dirs_to_delete = []
    space_required = 70000000 - tree.children[0].size
    space_required = 30000000 - space_required
    for pre, fill, node in RenderTree(tree):
        print("%s%s" % (pre, (node.id, node.size, node.depth)))
        if node.size < 100000:
            answer += node.size
        if node.size >= space_required:
            dirs_to_delete.append(node)

    smallest_node_size = 9999999999
    smallest_node = None
    for dir_d in dirs_to_delete:
        if dir_d.size < smallest_node_size:
            smallest_node_size = dir_d.size
            smallest_node = dir_d

    print(space_required)
    print(smallest_node.id)
    print(smallest_node.size)
    print(answer)



# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    print_hi()

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
#42036703
#70000000
#27963297
#2036703