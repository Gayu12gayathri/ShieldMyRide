import axios from "axios";

const backendUrl = "https://localhost:7153/api/Proposals/download-pdf"

export const downloadPolicyPDF = async (proposalId, vehicleRegNo) => {
  try {
    const token = localStorage.getItem("token");

    const response = await axios.get(
      `${backendUrl}/${proposalId}/${vehicleRegNo}`,
      {
        responseType: "blob",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    // Create download link
    const url = window.URL.createObjectURL(new Blob([response.data]));
    const link = document.createElement("a");
    link.href = url;
    link.setAttribute(
      "download",
      `Policy_${vehicleRegNo}_${proposalId}.pdf`
    );
    document.body.appendChild(link);
    link.click();
    link.remove();
  } catch (error) {
    console.error("Error downloading policy:", error);
    alert("Failed to download policy document. Please try again later.");
  }
};
