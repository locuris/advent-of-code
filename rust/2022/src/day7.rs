use std::ptr::null;
use crate::util;
use indextree;
use indextree::Arena;

struct Directory<'id> { id: &'id str, size: i32, depth: i32}

pub fn print_results() {
    let input = util::parse_input("day_7.txt");
    let lines = input.lines();
    let arena = &mut Arena::new();
    let root = arena.new_node(Directory{ id: "/", size: 0, depth: 0});
    let mut cd = root;
    for line in lines {
        let mut depth = 0;
        if line.starts_with("$ cd") {
            if line.starts_with("$ cd ..") {
                depth -= 1;
                cd = arena[cd].parent().unwrap();
            }
            else {
                depth += 1;
                //let other_line = line.clone();
                let dir = &line[5..];
                let new_cd = arena.new_node(Directory{ id: dir, size: 0, depth });
                cd.append(new_cd, arena);
                cd = new_cd;
            }
        }
        else if line.starts_with("dir") || line.starts_with("$") {
            continue
        }
        else {
            let size: i32 = line.split(" ").collect::<Vec<&str>>()[0].parse().unwrap();
            let dir = arena[cd].get_mut();
            dir.size += size;
            let mut parent = arena[cd].parent();
            while parent.is_some() {
                arena[parent.unwrap()].get_mut().size += size;
                parent = arena[parent.unwrap()].parent();
            }
        }
    }

    let mut answer_part1 = 0;
    let mut space_required = 70000000 - arena[root].get().size;
    space_required = 30000000 - space_required;
    let mut smallest_directory = 999999999;
    for directory in arena.iter() {
        let dir = directory.get();
        if dir.size <= 100000 {
            answer_part1 += dir.size;
        }
        if dir.size >= space_required && dir.size < smallest_directory {
            smallest_directory = dir.size;
        }
    }

    println!("{}", answer_part1);
    println!("{}", smallest_directory);

}