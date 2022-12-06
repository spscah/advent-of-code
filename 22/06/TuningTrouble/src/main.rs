use std::collections::HashSet;
use std::fs::File;
use std::io::prelude::*;

fn main() {
    let testdata = single_string("test.txt".to_string());
    let one = calculation(testdata.clone(), true);
    assert_eq!(one, 7);
    let two = calculation(testdata.clone(), false);
    assert_eq!(two, 19);

    let realdata = single_string("today.txt".to_string());
    let one = calculation(realdata.clone(), true);
    let two = calculation(realdata.clone(), false);
    println!("part one: {}", one);
    println!("part two: {}", two);
}

fn calculation(testdata: String, arg: bool) -> usize {
    let step = if arg { 4 } else { 14 };
    let mut i = 0;

    while i < testdata.len() {
        let mut j = i + step;
        if j > testdata.len() {
            j = testdata.len();
        }
        let slice = &testdata[i..j];
        let set: HashSet<char> = slice.chars().collect();
        if set.len() == step {
            return j;
        }
        i += 1;
    }
    0
}

fn single_string(filename: String) -> String {
    let mut f = File::open(filename).expect("file not found");
    let mut contents = String::new();
    f.read_to_string(&mut contents)
        .expect("something went wrong reading the file");
    contents
}
