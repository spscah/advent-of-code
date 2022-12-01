use std::fs::File;
use std::io::{self, BufRead};
use std::path::Path;

fn main() {
    let testdata = collect_integers("test.txt".to_string());

    // tuples: https://doc.rust-lang.org/rust-by-example/primitives/tuples.html
    let (one, two) = calculation(testdata);
    assert_eq!(one, 24000);
    assert_eq!(two, 45000);

    let realdata = collect_integers("data.txt".to_string());
    let (one, two) = calculation(realdata);
    println!("part one: {}", one);
    println!("part two: {}", two);
}

fn calculation(numbers: Vec<String>) -> (i32, i32) {
    let mut m1 = 0;
    let mut m2 = 0;
    let mut m3 = 0;
    let mut current = 0;
    for number in &numbers {
        if number.trim().len() == 0 {
            if current > m1 {
                m3 = m2;
                m2 = m1;
                m1 = current;
            } else if current > m2 {
                m3 = m2;
                m2 = current;
            } else if current > m3 {
                m3 = current;
            }
            current = 0;
        } else {
            // https://stackoverflow.com/a/59659615/2902
            let value: i32 = number.trim().parse().unwrap();
            current += value;
        }
    }
    (m1, m1 + m2 + m3)
}

// based on https://doc.rust-lang.org/rust-by-example/std_misc/file/read_lines.html
fn collect_integers(filename: String) -> Vec<String> {
    let mut numbers = Vec::new();

    if let Ok(lines) = read_lines(&filename) {
        // Consumes the iterator, returns an (Optional) String
        for line in lines {
            if let Ok(ip) = line {
                numbers.push(ip);
            }
        }
    }
    numbers.push("".to_string());
    numbers
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
