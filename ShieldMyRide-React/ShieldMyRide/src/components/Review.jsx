import React, { useState } from "react";
import "./Review.css";

const reviewsData = [
  {
    name: "Mr Giridhara Kini & Mrs Pravina",
    role: "Health Insurance",
    rating: 5,
    comment:
      "We want to sincerely thank SBI General Insurance for the timely transfer of the net claim amount to cover the various risks for our property at Bengaluru."
  },
  {
    name: "Prof. Debabrata Roy",
    role: "Health Reimbursement",
    rating: 5,
    comment:
      "I am very much happy with your staff who explained all the claim related issues clearly. Such polite staff will definitely increase your business."
  },
  {
    name: "Mylavarapu Venkataramana",
    role: "Claims",
    rating: 5,
    comment:
      "I had wonderful experience of lodging claim as well as updating drivers name and licence numbers."
  },
  {
    name: "Rahul Verma",
    role: "Two Wheeler Insurance",
    rating: 4,
    comment:
      "The representative was very helpful, patient, and clear while explaining the two wheeler policy. Thanks!"
  },
  {
    name: "Priya Singh",
    role: "Health Insurance",
    rating: 5,
    comment:
      "I experienced a claim process with zero stress. No delays, no complications. Just professional service."
  },
  {
    name: "Anil Mehta",
    role: "Car Insurance",
    rating: 4,
    comment:
      "Very satisfied with coverage and quick response. I highly recommend ShieldMyRide."
  }
];

export default function Review() {
  const [startIndex, setStartIndex] = useState(0);
  const reviewsPerPage = 3;

  const prevSlide = () => {
    setStartIndex((prev) => Math.max(prev - reviewsPerPage, 0));
  };

  const nextSlide = () => {
    setStartIndex((prev) =>
      prev + reviewsPerPage < reviewsData.length ? prev + reviewsPerPage : prev
    );
  };

  const visibleReviews = reviewsData.slice(
    startIndex,
    startIndex + reviewsPerPage
  );

  return (
    <section className="reviews-section">
      <h2 className="reviews-title">Why Customers Love Us?</h2>
      <div className="reviews-wrapper">
        {/* Left Arrow (only show if not at start) */}
        {startIndex > 0 && (
          <button className="arrow-btn left" onClick={prevSlide}>
            &#10094;
          </button>
        )}

        {/* Review Cards */}
        <div className="review-cards">
          {visibleReviews.map((review, index) => (
            <div className="review-card" key={index}>
              <p className="comment">“{review.comment}”</p>
              <h4>{review.name}</h4>
              <span className="role">{review.role}</span>
              <div className="stars">
                {"★".repeat(review.rating)}
                {"☆".repeat(5 - review.rating)}
              </div>
            </div>
          ))}
        </div>

        {/* Right Arrow (only show if more reviews left) */}
        {startIndex + reviewsPerPage < reviewsData.length && (
          <button className="arrow-btn right" onClick={nextSlide}>
            &#10095;
          </button>
        )}
      </div>
    </section>
  );
}
