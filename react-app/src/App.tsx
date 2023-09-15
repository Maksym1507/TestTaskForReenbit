import React, { useEffect, useState } from 'react';
import './App.css';
import "bootstrap/dist/css/bootstrap.min.css";
import UploadFileRequest from './models/requests/UploadFileRequest';
import { Button, Form } from 'react-bootstrap';
import { useForm } from 'react-hook-form';
import UploadFileResponse from './models/responses/UploadFileResponse';

function App() {
  const [uploadFileResponse, setUploadFileResponse] = useState<UploadFileResponse>();

  useEffect(() => {
    debugger;
    if (uploadFileResponse) {
      alert(uploadFileResponse.message);
    }
  }, [uploadFileResponse]);

  const {
    register: registerUploadLoadFileForm,
    formState: { errors: errorsForUploadLoadFileForm, isValid },
    handleSubmit: handleSubmitUploadLoadFileForm,
    reset
  } = useForm({ mode: "onBlur" });

  const [uploadFileRequest, setUploadFileRequest] = useState<UploadFileRequest>({
  } as UploadFileRequest);

  const handleChangeEmail = (e: any) => {
    setUploadFileRequest({ ...uploadFileRequest, [e.target.name]: e.target.value });
  };

  const handleChangeFile = (e: any) => {
    console.log(e.target.files[0].name);
    setUploadFileRequest({ ...uploadFileRequest, formFile: e.target.files[0], fileName: e.target.files[0].name });
  };

  const handleSubmit = async () => {
    try {
      const formData = new FormData();
      formData.append("email", uploadFileRequest.email);
      formData.append("formFile", uploadFileRequest.formFile);
      formData.append("fileName", uploadFileRequest.fileName);

      setUploadFileRequest({} as UploadFileRequest);
      reset();

      const requestOptions = {
        method: 'POST',
        body: formData
      };

      const res = await fetch("https://testtaskforreenbit.azurewebsites.net/BlobStorage/AddFileToBlobStorage", requestOptions);
      setUploadFileResponse(await res.json());
    }
    catch (ex) {
      console.log(ex);
    }
  };

  return (
    <div className="App container h-100">
      <div className="row d-flex justify-content-center align-items-center h-100">
        <h2>Form gefor addition file in Blob Stora</h2>
        <Form
          noValidate
          className="ms-3 mb-3 pt-2 pb-2"
          style={{ width: "24rem" }}
          onSubmit={handleSubmitUploadLoadFileForm(handleSubmit)}
        >
          <Form.Group controlId="formEmail">
            <Form.Label className="d-flex">
              Email address
            </Form.Label>
            <Form.Control
              style={{ width: "21rem" }}
              defaultValue={""}
              type="email"
              placeholder="Enter email"
              {...registerUploadLoadFileForm("email", {
                required: "Email can not be empty",
                pattern: {
                  value: /^[a-zA-Z0-9].+@[a-zA-Z0-9]+\.[A-Za-z]+$/,
                  message: "Invalid email format",
                },
              })}
              onChange={(e: any) => handleChangeEmail(e)}
            />
            <div style={{ height: 20 }}>
              {errorsForUploadLoadFileForm?.email && (
                <p className="text-danger text-center">
                  {errorsForUploadLoadFileForm?.email?.message?.toString()}
                </p>
              )}
            </div>
          </Form.Group>

          <Form.Group controlId="formFile">
            <Form.Label className="d-flex">
              Choose a File:
            </Form.Label>
            <Form.Control
              style={{ width: "21rem" }}
              type="file"
              autoComplete="on"
              {...registerUploadLoadFileForm("fileName", {
                required: "File must be selected"
              })}
              onChange={(e: any) => handleChangeFile(e)}
              accept=".docx"
            />
            <div style={{ height: 20 }}>
              {errorsForUploadLoadFileForm?.fileName && (
                <p className="text-danger text-center">
                  {errorsForUploadLoadFileForm?.fileName?.message?.toString()}
                </p>
              )}
            </div>
          </Form.Group>
          <div className="text-center">
            <Button
              className="ms-3 mt-2 center-block btn-success"
              style={{ width: "7rem" }}
              variant="primary"
              type="submit"
              disabled={!isValid}
            >
              Submit
            </Button>
          </div>
        </Form>
      </div>
    </div >
  );
}

export default App;
