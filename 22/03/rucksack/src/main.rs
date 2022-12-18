use std::collections::HashSet;
use std::fs::File;
use std::io::{self, BufRead};
use std::path::Path;

fn main() {
    let testdata = collect_strings("test.txt".to_string());
    let (one, two) = calculation(testdata);
    assert_eq!(one, 157);
    // assert_eq!(two, 70);
}

fn calculation(rucksacks: Vec<String>) -> (i32, i32) {
    let mut score1: i32 = 0;
    let mut score2: i32 = 0;

    for rucksack in &rucksacks {
        let mid = rucksack.len() / 2;
        let (first, second) = rucksack.split_at(mid);

        let f: HashSet<char> = first.chars().collect();
        let s: HashSet<char> = second.chars().collect();

        let mut i = f.intersection(&s);

        assert_eq!(i.count(), 1);

        for ch in &i.next() {
            let lower = 'a' as i32;
            let upper = 'B' as i32;

            if ch.is_lowercase() {
                score1 += (ch as *const char) as i32 - lower + 1;
            } else {
                score1 += (ch as u8) as i32 - upper + 1;
            }
        }
    }

    (score1, score2)
}

fn collect_strings(filename: String) -> Vec<String> {
    let mut strings = Vec::new();

    if let Ok(lines) = read_lines(&filename) {
        // Consumes the iterator, returns an (Optional) String
        for line in lines {
            if let Ok(data) = line {
                strings.push(data);
            }
        }
    }
    strings.push("".to_string());
    strings
}

// The output is wrapped in a Result to allow matching on errors
// Returns an Iterator to the Reader of the lines of the file.
fn read_lines<P>(filename: P) -> io::Result<io::Lines<io::BufReader<File>>>
where
    P: AsRef<Path>,
{
    let file = File::open(filename)?;
    Ok(io::BufReader::new(file).lines())
}
