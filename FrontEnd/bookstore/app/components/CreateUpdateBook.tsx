import Modal from "antd/es/modal/Modal";
import { BookRequest } from "../services/books";
import { title } from "process";
import { SetStateAction, useEffect, useState } from "react";
import TextArea from "antd/es/input/TextArea";

interface Props {
    mode: Mode;
    values: Book;
    isModalOpen: boolean;
    handleCancel: () => void;
    handleCreate: (request: BookRequest) => void;
    handleUpdate: (id: string, request: BookRequest) => void;
}

export enum Mode {
    Create = "create", 
    Edit = "edit",
}

export const CreateUpdateBook = ({
    mode,
    values,
    isModalOpen,
    handleCancel,
    handleCreate,
    handleUpdate,

}: Props) => {
    const [title, setTitle] = useState<string>("");
    const [description, setDescription] = useState<string>("");
    const [price, setPrice] = useState<number>(1);

    useEffect(() => {
        if (mode === Mode.Edit && values) {
            setTitle(values.title);
            setDescription(values.description);
            setPrice(values.price);
        } else {
            // Очищаем поля при создании
            setTitle("");
            setDescription("");
            setPrice(1);
        }
    }, [mode, values]);

    const handleOnOk = () => {
        const bookRequest = { title, description, price };
        if (mode === Mode.Create) {
            handleCreate(bookRequest);
        } else {
            handleUpdate(values.id, bookRequest);
        }
    };

    return (
        <Modal 
        title={mode == Mode.Create ? "Add Book" : "Edit Book"} 
        open={isModalOpen} 
        cancelText="Cancel"
        onOk={handleOnOk} 
        onCancel={handleCancel}>

            <div className="book__modal">
                <input
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                placeholder="Name"></input>
                <TextArea
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                autoSize={{minRows: 3, maxRows: 3}}
                placeholder="Descriprion"></TextArea>
                <input
                value={price}
                onChange={(e) => setPrice(Number(e.target.value))}
                placeholder="Price"></input>
            </div>
        </Modal>
    )
}