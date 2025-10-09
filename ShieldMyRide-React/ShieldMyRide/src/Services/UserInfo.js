import axios from "axios";

const API_URL = "https://localhost:7153/api/User"

const handleError = (error) => {
    if(error.response){
        throw new Error(
            `Server Error (${error.response.status}): ${error.response.data?.message || error.message}`
        );
    } else if (error.request) {
        throw new Error (
            "No Response from Server.Please Check the server or Try after sometime"
        );
    } else {
        throw new Error(
            `Error: ${error.message}`
        );
    }
};



// export const getUserDetails = async (role, id) => {
//   try {
//     const response = await axios.get(`${API_URL}/${role.toLowerCase()}/${id}`);
//     return response.data;
//   } catch (error) {
//     console.error("Error fetching user details:", error);
//     throw error;
//   }
// };

//logged in as customer show details of the customer
export const getCustomer = async (id) => {
    try{
        console.log ("id in getcustomerById "+ id)
        const response = await axios.get(`${API_URL}/customer/${id}`);
        return response.data;
    } catch (error) {
        handleError(error);
    }
};

//logged in as officer  show the deatils of teh officer
export const getOfficer = async (id) => {
    try{
        console.log ("id in getOfficerById "+ id)
        const response = await axios.get(`${API_URL}/officer/${id}`);
        return response.data;
    } catch (error) {
        handleError(error);
    }
}

// logged in as admin show the details of the admin and the user 
export const getAdmin = async (id) =>{
    try{
        console.log ("id in getAdminById "+ id)
        const response = await axios.get(`${API_URL}/admin/${id}`);
        return response.data;
    } catch (error) {
        handleError(error);
    }
}

export const getUserByRole = async (customerUserId) =>{
    try{
        console.log ("customerUserId in getUserByRole "+ customerUserId)
        const response = await axios.get(`${API_URL}/${role}/${customerUserId}`);
        return response.data;
    } catch (error) {
        handleError(error);
    }
}

export const getUserBalance = async (userId) =>{
    try{
        console.log ("GetUserBalance in  "+ userId)
        const response = await axios.get(`${API_URL}/user/${userId}/balance`);
        return response.data;
    } catch (error) {
        handleError(error);
    }
}