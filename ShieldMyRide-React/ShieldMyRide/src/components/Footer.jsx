import React from "react";
import { useNavigate } from "react-router-dom";
import { FaFacebook, FaTwitter, FaInstagram, FaLinkedin } from "react-icons/fa";
import "bootstrap/dist/css/bootstrap.min.css";
import "./Footer.css";

export default function Footer() {
  const navigate = useNavigate();

  return (
    <footer className="footer-container">
      <div className="footer-content container">
        {/* Quick Links */}
        <div className="footer-section">
          <h5 className="footer-title">Quick Links</h5>
          <ul className="footer-links">
            <li onClick={() => navigate("/insurancepolicies")}>Insurance Policies</li>
            <li onClick={() => navigate("/claims")}>Claims</li>
            <li onClick={() => navigate("/quote")}>Calculate Quote</li>
            <li onClick={() => navigate("/download-policy")}>Download Policy</li>
            {/* <li onClick={() => navigate("/address")}>Address</li> */}
            <li onClick={() => navigate("/queries")}>Queries</li>
          </ul>
        </div>

        {/* Social Media */}
        <div className="footer-section">
          <h5 className="footer-title">Follow Us</h5>
          <div className="social-icons">
            <a href="https://facebook.com" target="_blank" rel="noreferrer">
              <FaFacebook />
            </a>
            <a href="https://twitter.com" target="_blank" rel="noreferrer">
              <FaTwitter />
            </a>
            <a href="https://instagram.com" target="_blank" rel="noreferrer">
              <FaInstagram />
            </a>
            <a href="https://linkedin.com" target="_blank" rel="noreferrer">
              <FaLinkedin />
            </a>
          </div>
        </div>

        {/* Copyright */}
        <div className="footer-section">
          <h5 className="footer-title">ShieldMyRide</h5>
          <p className="footer-text">
            Your trusted partner for safe and reliable motor insurance.
          </p>
          <p className="footer-copy">
            © {new Date().getFullYear()} ShieldMyRide. All Rights Reserved.
          </p>
        </div>
      </div>
    </footer>
  );
}
