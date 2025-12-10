use advent::File;

// Adds `value` if it isn't in `ordered_list`, removes it if it is, maintaining the order.
fn toggle_value_membership_in_ordered_list(ordered_list: &mut Vec<usize>, value: usize) {
    let mut i = 0;

    while i < ordered_list.len() && ordered_list[i] < value {
        i += 1;
    }
    
    if i == ordered_list.len() || ordered_list[i] != value {
        ordered_list.insert(i, value);
    } else {
        ordered_list.remove(i);
    }
}

/// The set { x in usize | l <= x <= r }.
#[derive(Clone, Copy, Debug)]
struct Interval {
    l: usize,
    r: usize,
}

impl Interval {
    fn new(l: usize, r: usize) -> Self {
        debug_assert!(l <= r);

        Interval { l, r }
    }

    fn intersects(self, other: Self) -> bool {
        other.l <= self.r && self.l <= other.r
    }

    fn intersection(self, other: Self) -> Self {
        debug_assert!(self.intersects(other));

        Interval::new(self.l.max(other.l), self.r.min(other.r))
    }

    fn contains(self, x: usize) -> bool {
        self.l <= x && x <= self.r
    }

    #[inline]
    fn update_intervals_from_scanline_bounds(scanline_bounds: &Vec<usize>, to_update: &mut IntervalSet) {
        debug_assert!(scanline_bounds.len() % 2 == 0);

        to_update.intervals.clear();

        let mut it = scanline_bounds.iter();

        while let (Some(&l), Some(&r)) = (it.next(), it.next()) {
            to_update.intervals.push(Interval::new(l, r));
        }
    }
}

/// A multiset of `Interval`s, with no invariants (such as disjointness) placed on the membership.
#[derive(Clone, Debug)]
struct IntervalSet {
    intervals: Vec<Interval>,
}

impl IntervalSet {
    /// Returns an arbitrary interval in the multiset that contains `x`, or `None` if there isn't one.
    fn containing(&self, x: usize) -> Option<Interval> {
        for &interval in self.intervals.iter() {
            if interval.contains(x) {
                return Some(interval);
            }
        }
    
        return None
    }
}

fn main() {
    let input = File::new("real-input.txt");

    // Load the coordinates into "scanline" order, i.e. top to bottom then left to right:
    let vectors = input.parsed_lines(|line| line.split(',').map(|number| number.parse::<usize>().ok()).collect::<Option<Vec<_>>>());
    let coordinates: Vec<_> = vectors.map(|vector| (vector[0], vector[1])).collect();

    let solve = |coordinates: &Vec<(usize, usize)>| {
        // To ensure the sorting is included in the benchmark:
        let mut coordinates = coordinates.clone();
        coordinates.sort_unstable_by_key(|&(x, y)| (y, x));

        // Track the largest area so far during scanning:
        let mut largest_area = 0usize;

        // Each red tile (`x`, `y`) becomes a candidate for being a top corner of the largest area, and during the 
        // scan the `interval` containing the maximum possible width is updated:
        #[derive(Debug)]
        struct Candidate {
            x: usize,
            y: usize,
            interval: Interval,
        }

        let mut candidates: Vec<Candidate> = vec![];
        
        // Maintain an ordered list of scanline points, i.e. [interval_begin_0, interval_end_0, interval_begin_1, interval_end_1, ...]:
        let mut scanline_bounds: Vec<usize> = vec![];
        let mut intervals_from_scanlines = IntervalSet { intervals: vec![] };

        // Invariants on the input data (defined by the puzzle) result in points arriving in pairs on the same y line:
        let mut it = coordinates.into_iter();

        while let (Some((x0, y)), Some((x1, y1))) = (it.next(), it.next()) {
            assert_eq!(y, y1);
        
            // Update the scanline points:
            for x in [x0, x1] {
                toggle_value_membership_in_ordered_list(&mut scanline_bounds, x);
            }
        
            // Find the resulting intervals on this line:
            Interval::update_intervals_from_scanline_bounds(&scanline_bounds, &mut intervals_from_scanlines);
        
            // Check the produced rectangles within the candidates:
            for candidate in candidates.iter() {
                for x in [x0, x1] {
                    if candidate.interval.contains(x) {
                        largest_area = largest_area.max((candidate.x.abs_diff(x) + 1) * (candidate.y.abs_diff(y) + 1));
                    }
                }
            }
        
            // Update and drop candidates:
            candidates.retain_mut(|candidate| {
                if let Some(intersection_containing_x) = intervals_from_scanlines.containing(candidate.x) {
                    candidate.interval = intersection_containing_x.intersection(candidate.interval);

                    true
                } else {
                    false
                }
            });
            
            // Add any new candidates:
            for x in [x0, x1] {
                if let Some(containing) = intervals_from_scanlines.containing(x) {
                    candidates.push(Candidate { x, y, interval: containing });
                }
            }
        }
    
        assert_eq!(largest_area, 1573359081);
    };

    let options = microbench::Options::default();

    microbench::bench(&options, "solve", || solve(&coordinates));
}
main.rs
6 KB